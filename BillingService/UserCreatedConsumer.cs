using DAL;
using MassTransit;
using MQ;

namespace BillingService;

public class UserCreatedConsumer: IConsumer<UserCreated>
{
    private readonly BillingDbContext _dbContext;

    public UserCreatedConsumer(BillingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        _dbContext.Accounts.Add(new Account
        {
            Id = context.Message.UserId,
            CreditCard = context.Message.CreditCard
        });
        await _dbContext.SaveChangesAsync();
    }
}