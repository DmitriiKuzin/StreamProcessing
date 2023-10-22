namespace DAL;

public class Order
{
    public long Id { get; set; }
    public double Price { get; set; }
    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
}