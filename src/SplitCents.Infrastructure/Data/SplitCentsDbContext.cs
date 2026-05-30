namespace SplitCents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class SplitCentsDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("gabrgopaebgb");
    }
}