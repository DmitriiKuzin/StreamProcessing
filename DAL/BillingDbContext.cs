using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DAL;

public class BillingDbContext: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = new NpgsqlConnectionStringBuilder();
        a.Host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        a.Database = "billing";
        a.Username = "postgres";
        a.Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        optionsBuilder.UseNpgsql(a.ToString());
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountTransaction> AccountTransactions { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}