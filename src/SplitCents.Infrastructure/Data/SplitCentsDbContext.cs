namespace SplitCents.Infrastructure.Data;
using SplitCents.Core.Models;
using Microsoft.EntityFrameworkCore;

public class SplitCentsDbContext : DbContext
{
    public SplitCentsDbContext(DbContextOptions<SplitCentsDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
}