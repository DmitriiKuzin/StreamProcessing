namespace DAL;

public class Notification
{
    public long Id { get; set; }
    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; }
    public string Message { get; set; }
}