namespace SplitCents.Infrastructure.Tests.Integration;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SplitCents.Infrastructure.Data;
using FluentAssertions;
using Xunit;

public class DbConnectionTests : IDisposable
{
    private readonly SplitCentsDbContext _db;

    public DbConnectionTests()
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.Development.json", optional: false)
        .Build();

        var options = new DbContextOptionsBuilder<SplitCentsDbContext>()
            .UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .Options;

        _db = new SplitCentsDbContext(options);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task CanConnectToDatabase()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        canConnect.Should().BeTrue();
    }

    public void Dispose() => _db.Dispose();
}