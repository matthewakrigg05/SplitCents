namespace SplitCents.API.Tests.Controllers;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using SplitCents.Core.DTOs;
using SplitCents.Core.Exceptions;
using SplitCents.Core.Interfaces.Services;
using SplitCents.Infrastructure.Data;
using SplitCents.API.DTOs;
using FluentAssertions;
using Xunit;

// Full-pipeline factory - SQLite connection kept open so data persists within
// a test (all requests share the same store).
internal class SplitCentsApiFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly SqliteConnection _connection;

    public SplitCentsApiFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-secret-key-splitcents-at-least-32-chars!!",
                ["Jwt:Issuer"] = "SplitCents",
                ["Jwt:Audience"] = "SplitCents",
                ["Jwt:ExpiryHours"] = "1"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(SplitCentsDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<SplitCentsDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions))
                .ToList();
            foreach (var d in toRemove) services.Remove(d);

            var dbOptions = new DbContextOptionsBuilder<SplitCentsDbContext>()
                .UseSqlite(_connection)
                .Options;

            services.AddSingleton(_ =>
            {
                var ctx = new SplitCentsDbContext(dbOptions);
                ctx.Database.EnsureCreated();
                return ctx;
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _connection.Dispose();
    }
}

// Mocked-service factory - used to test API-layer concerns without a database.
internal class MockUserServiceFactory : WebApplicationFactory<Program>
{
    private readonly Mock<IUserService> _userServiceMock;

    public MockUserServiceFactory(Mock<IUserService> userServiceMock)
    {
        _userServiceMock = userServiceMock;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-secret-key-splitcents-at-least-32-chars!!",
                ["Jwt:Issuer"] = "SplitCents",
                ["Jwt:Audience"] = "SplitCents",
                ["Jwt:ExpiryHours"] = "1"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(SplitCentsDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<SplitCentsDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions))
                .ToList();
            foreach (var d in toRemove) services.Remove(d);

            services.AddDbContext<SplitCentsDbContext>(o =>
                o.UseInMemoryDatabase("MockTests_" + Guid.NewGuid()));

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserService));
            if (descriptor != null) services.Remove(descriptor);
            services.AddSingleton(_userServiceMock.Object);
        });
    }
}

// Each test gets its own factory instance (fresh DB) for isolation.
public class UsersControllerTests : IDisposable
{
    private readonly SplitCentsApiFactory _factory;
    private readonly HttpClient _client;

    public UsersControllerTests()
    {
        _factory = new SplitCentsApiFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    // -- Full pipeline --------------------------------------------------------

    [Fact]
    public async Task Register_WithValidData_Returns201WithTokenAndUser()
    {
        var request = new RegisterRequest
        {
            Email = "newuser@example.com",
            Password = "Secure@99",
            DisplayName = "newuser",
            FirstName = "John",
            LastName = "Doe"
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<RegisterResponse>();
        body.Should().NotBeNull();
        body!.Token.Should().NotBeNullOrWhiteSpace();
        body.User.email.Should().Be("newuser@example.com");
        body.User.displayName.Should().Be("newuser");
    }

    // -- Middleware / contract (mocked service) --------------------------------

    [Fact]
    public async Task Register_WhenServiceThrowsValidationException_Returns400WithErrorBody()
    {
        var mock = new Mock<IUserService>();
        mock.Setup(s => s.RegisterAsync(
                It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ThrowsAsync(new ValidationException("Email is already in use."));

        using var factory = new MockUserServiceFactory(mock);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/users/register", new RegisterRequest
        {
            Email = "taken@example.com",
            Password = "Secure@99",
            DisplayName = "someuser"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Email is already in use.");
    }

    [Fact]
    public async Task Register_WithInvalidEmail_Returns400()
    {
        var request = new RegisterRequest
        {
            Email = "notanemail",
            Password = "Secure@99",
            DisplayName = "someuser"
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithWeakPassword_Returns400()
    {
        var request = new RegisterRequest
        {
            Email = "user@example.com",
            Password = "weak",
            DisplayName = "someuser"
        };

        var response = await _client.PostAsJsonAsync("/api/users/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
