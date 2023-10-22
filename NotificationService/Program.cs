using DAL;
using MQ;
using NotificationService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<BillingDbContext>();
        services.AddRabbitMq(x =>
        {
            x.AddConsumer<EnoughFundsConsumer>();
            x.AddConsumer<InsufficientFundsConsumer>();
        });
    })
    .Build();

await MqExtension.WaitForRabbitReady();
host.Run();