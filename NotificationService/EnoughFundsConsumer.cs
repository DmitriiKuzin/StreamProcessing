using DAL;
using MassTransit;
using MQ;

namespace NotificationService;

public class EnoughFundsConsumer: IConsumer<EnoughFunds>
{
    private readonly BillingDbContext _dbContext;

    public EnoughFundsConsumer(BillingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EnoughFunds> context)
    {
        _dbContext
            .Notifications
            .Add(new Notification
            {
                OrderId = context.Message.OrderId,
                UserProfileId = context.Message.UserProfileId,
                Message = "письмо счастья !"
            });
        await _dbContext.SaveChangesAsync();
    }
}