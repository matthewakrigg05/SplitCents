namespace SplitCents.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Models;
using SplitCents.Infrastructure.Data.Configurations;

public class SplitCentsDbContext : DbContext
{
    public SplitCentsDbContext(DbContextOptions<SplitCentsDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
