namespace SplitCents.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Models;
using SplitCents.Infrastructure.Data.Configurations;

public class SplitCentsDbContext : DbContext
{
    public SplitCentsDbContext(DbContextOptions<SplitCentsDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }
    public DbSet<UserTransactionCategory> UserTransactionCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new UserTransactionCategoryConfiguration());
    }
}
