using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace BillingService;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly BillingDbContext _dbContext;
    private readonly IBus _bus;

    public OrderCreatedConsumer(BillingDbContext dbContext, IBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var currentAccountAmount = await _dbContext
            .AccountTransactions
            .AsNoTracking()
            .Where(x => x.AccountId == context.Message.UserProfileId)
            .SumAsync(x => x.Amount);
        if (currentAccountAmount < context.Message.Price)
        {
            await _bus.Publish(new InsufficientFunds(context.Message.OrderId, context.Message.UserProfileId));
            return;
        }

        await _bus.Publish(new EnoughFunds(context.Message.OrderId, context.Message.UserProfileId));
        _dbContext
            .AccountTransactions
            .Add(new AccountTransaction
            {
                AccountId = context.Message.UserProfileId,
                Amount = -context.Message.Price,
                DateTime = DateTime.UtcNow
            });
        await _dbContext.SaveChangesAsync();
    }
}