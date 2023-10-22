namespace MQ;

public record UserCreated(long UserId, string CreditCard);
public record OrderCreated(long OrderId, double Price, long UserProfileId);

public record InsufficientFunds(long OrderId, long UserProfileId);

public record EnoughFunds(long OrderId, long UserProfileId);