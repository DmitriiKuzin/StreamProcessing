using DAL;
using MassTransit;
using MQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BillingDbContext>();
builder.Services.AddRabbitMq();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/userProfile", async (UserProfile userProfile, BillingDbContext dbContext, IBus bus) =>
{
    dbContext.UserProfiles.Add(userProfile);
    await dbContext.SaveChangesAsync();
    await bus.Publish(new UserCreated(userProfile.Id, userProfile.CreditCard));
});


await MqExtension.WaitForRabbitReady();
app.Run();