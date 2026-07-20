namespace SplitCents.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Models;
using SplitCents.Infrastructure.Data.Configurations;

public class SplitCentsDbContext : DbContext
{
    public SplitCentsDbContext(DbContextOptions<SplitCentsDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<RecurringSubscription> RecurringSubscriptions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BudgetCategory> BudgetCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BillConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringSubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetCategoryConfiguration());
    }
}
