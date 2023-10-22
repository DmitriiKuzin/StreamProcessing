using BillingService;
using DAL;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BillingDbContext>();
builder.Services.AddRabbitMq(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();
    x.AddConsumer<UserCreatedConsumer>();
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/ReplenishAccount", async ([FromQuery] long userProfileId, [FromQuery] double amount, BillingDbContext dbContext, IBus bus) =>
{
    var accountTransaction = new AccountTransaction
    {
        Amount = amount,
        DateTime = DateTime.UtcNow,
        AccountId = userProfileId
    };
    dbContext.AccountTransactions.Add(accountTransaction);
    await dbContext.SaveChangesAsync();
});

app.MapGet("currentAmount", async ([FromQuery] long userProfileId, BillingDbContext dbContext) =>
{
    var currentAccountAmount = await dbContext
        .AccountTransactions
        .AsNoTracking()
        .Where(x => x.AccountId == userProfileId)
        .SumAsync(x => x.Amount);
    return currentAccountAmount;
});

await MqExtension.WaitForRabbitReady();
app.Run();