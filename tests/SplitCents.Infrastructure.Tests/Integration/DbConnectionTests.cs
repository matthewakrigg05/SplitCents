namespace SplitCents.Infrastructure.Tests.Integration;

using Microsoft.EntityFrameworkCore;
using SplitCents.Infrastructure.Data;
using FluentAssertions;
using Xunit;

public class DbConnectionTests : IDisposable
{
    private readonly SplitCentsDbContext _db;

    public DbConnectionTests()
    {
        var options = new DbContextOptionsBuilder<SplitCentsDbContext>()
            .UseNpgsql("Host=localhost;Database=SpltCntsDev;Username=SplitCentsApplication;Password=splitcentsdev")
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