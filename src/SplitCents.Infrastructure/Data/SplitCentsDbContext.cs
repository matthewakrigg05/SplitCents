namespace SplitCents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class SplitCentsDbContext : DbContext
{
    public SplitCentsDbContext(DbContextOptions<SplitCentsDbContext> options)
        : base(options) { }
}