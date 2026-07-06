namespace SplitCents.Infrastructure.Tests.Unit;

using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Models;
using SplitCents.Infrastructure.Data;
using SplitCents.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

public class UserRepositoryTests : IDisposable
{
    private readonly SplitCentsDbContext _context;
    private readonly UserRepository _sut;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<SplitCentsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new SplitCentsDbContext(options);
        _sut = new UserRepository(_context);
    }

    private static User MakeUser(string email = "user@example.com", string displayName = "user1") => new()
    {
        id = Guid.NewGuid(),
        email = email,
        hashedPassword = "hashed",
        displayName = displayName,
        firstName = "John",
        lastName = "Doe"
    };

    [Fact]
    public async Task AddAsync_AddsUserToDatabase()
    {
        var user = MakeUser();
        await _sut.AddAsync(user);
        _context.Users.Should().ContainSingle(u => u.id == user.id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUser()
    {
        var user = MakeUser();
        await _sut.AddAsync(user);

        var result = await _sut.GetByIdAsync(user.id);

        result.Should().NotBeNull();
        result!.id.Should().Be(user.id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserNotFound_ReturnsNull()
    {
        var result = await _sut.GetByIdAsync(Guid.NewGuid());
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_WhenUserExists_ReturnsUser()
    {
        var user = MakeUser(email: "test@example.com");
        await _sut.AddAsync(user);

        var result = await _sut.GetByEmailAsync("test@example.com");

        result.Should().NotBeNull();
        result!.email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetByEmailAsync_WhenUserNotFound_ReturnsNull()
    {
        var result = await _sut.GetByEmailAsync("nonexistent@example.com");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByDisplayNameAsync_WhenUserExists_ReturnsUser()
    {
        var user = MakeUser(displayName: "testuser");
        await _sut.AddAsync(user);

        var result = await _sut.GetByDisplayNameAsync("testuser");

        result.Should().NotBeNull();
        result!.displayName.Should().Be("testuser");
    }

    [Fact]
    public async Task GetByDisplayNameAsync_WhenUserNotFound_ReturnsNull()
    {
        var result = await _sut.GetByDisplayNameAsync("nonexistent");
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var user = MakeUser();
        await _sut.AddAsync(user);

        user.displayName = "updated-name";
        await _sut.UpdateAsync(user);

        var updated = await _sut.GetByIdAsync(user.id);
        updated!.displayName.Should().Be("updated-name");
    }

    [Fact]
    public async Task DeleteAsync_RemovesUser()
    {
        var user = MakeUser();
        await _sut.AddAsync(user);

        await _sut.DeleteAsync(user);

        var deleted = await _sut.GetByIdAsync(user.id);
        deleted.Should().BeNull();
    }

    public void Dispose() => _context.Dispose();
}
