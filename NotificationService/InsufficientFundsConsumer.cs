using DAL;
using MassTransit;
using MQ;

namespace NotificationService;

public class InsufficientFundsConsumer: IConsumer<InsufficientFunds>
{
    private readonly BillingDbContext _dbContext;

    public InsufficientFundsConsumer(BillingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<InsufficientFunds> context)
    {
        _dbContext
            .Notifications
            .Add(new Notification
            {
                OrderId = context.Message.OrderId,
                UserProfileId = context.Message.UserProfileId,
                Message = "письмо горя =("
            });
        await _dbContext.SaveChangesAsync();
    }
}