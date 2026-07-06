namespace SplitCents.Core.Tests;

using Moq;
using FluentAssertions;
using Xunit;
using SplitCents.Core.Services;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Models.Security;
using SplitCents.Core.Models;
using SplitCents.Core.Exceptions;

public class UserServicesTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly UserService _sut;

    public UserServicesTests()
    {
        _sut = new UserService(_userRepoMock.Object, _passwordHasherMock.Object);
    }

    private static User MakeUser(Guid? id = null, string email = "user@example.com", string displayName = "user1") => new()
    {
        id = id ?? Guid.NewGuid(),
        email = email,
        hashedPassword = "hashed",
        displayName = displayName,
        firstName = "John",
        lastName = "Doe"
    };

    // ── RegisterAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_WithValidData_ReturnsUserResponse()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.GetByDisplayNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hashed_password");

        var result = await _sut.RegisterAsync("user@example.com", "Secure@99", "user1", "John", "Doe");

        result.Should().NotBeNull();
        result.email.Should().Be("user@example.com");
        result.displayName.Should().Be("user1");
        result.firstName.Should().Be("John");
        result.lastName.Should().Be("Doe");
        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.hashedPassword == "hashed_password")), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_NormalizesEmailToLowercase()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("user@example.com")).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.GetByDisplayNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hashed_password");

        var result = await _sut.RegisterAsync("user@example.com", "Secure@99", "user1", null, null);

        result.email.Should().Be("user@example.com");
        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.email == "user@example.com")), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ThrowsValidationException()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("user@example.com")).ReturnsAsync(MakeUser(email: "user@example.com"));

        var act = async () => await _sut.RegisterAsync("user@example.com", "Secure@99", "user1", null, null);

        await act.Should().ThrowAsync<ValidationException>().WithMessage("Email is already in use.");
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateDisplayName_ThrowsValidationException()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.GetByDisplayNameAsync("user1")).ReturnsAsync(MakeUser(displayName: "user1"));

        var act = async () => await _sut.RegisterAsync("user@example.com", "Secure@99", "user1", null, null);

        await act.Should().ThrowAsync<ValidationException>().WithMessage("Display name is already in use.");
    }

    [Fact]
    public async Task RegisterAsync_WithInvalidEmail_ThrowsValidationException()
    {
        var act = async () => await _sut.RegisterAsync("notanemail", "Secure@99", "user1", null, null);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RegisterAsync_WithInvalidPassword_ThrowsValidationException()
    {
        var act = async () => await _sut.RegisterAsync("user@example.com", "weak", "user1", null, null);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RegisterAsync_WithInvalidDisplayName_ThrowsValidationException()
    {
        var act = async () => await _sut.RegisterAsync("user@example.com", "Secure@99", "a", null, null);
        await act.Should().ThrowAsync<ValidationException>();
    }

    // ── GetUserByIdAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsUser()
    {
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(MakeUser(id: userId));

        var result = await _sut.GetUserByIdAsync(userId);

        result.Should().NotBeNull();
        result!.id.Should().Be(userId);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserNotFound_ReturnsNull()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var result = await _sut.GetUserByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    // ── UpdateDisplayNameAsync ─────────────────────────────────────────

    [Fact]
    public async Task UpdateDisplayNameAsync_WhenValid_UpdatesDisplayName()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId, displayName: "oldname");
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.GetByDisplayNameAsync("newname")).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        await _sut.UpdateDisplayNameAsync(userId, "newname");

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.displayName == "newname")), Times.Once);
    }

    [Fact]
    public async Task UpdateDisplayNameAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = async () => await _sut.UpdateDisplayNameAsync(Guid.NewGuid(), "newname");

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateDisplayNameAsync_WhenDisplayNameTakenByAnotherUser_ThrowsValidationException()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId, displayName: "oldname");
        var otherUser = MakeUser(id: Guid.NewGuid(), displayName: "newname");
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.GetByDisplayNameAsync("newname")).ReturnsAsync(otherUser);

        var act = async () => await _sut.UpdateDisplayNameAsync(userId, "newname");

        await act.Should().ThrowAsync<ValidationException>().WithMessage("Display name is already in use.");
    }

    [Fact]
    public async Task UpdateDisplayNameAsync_WhenSameDisplayNameForSameUser_DoesNotThrow()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId, displayName: "myname");
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.GetByDisplayNameAsync("myname")).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var act = async () => await _sut.UpdateDisplayNameAsync(userId, "myname");

        await act.Should().NotThrowAsync();
    }

    // ── UpdateEmailAsync ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateEmailAsync_WhenValid_UpdatesEmail()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId);
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        await _sut.UpdateEmailAsync(userId, "new@example.com");

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.email == "new@example.com")), Times.Once);
    }

    [Fact]
    public async Task UpdateEmailAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = async () => await _sut.UpdateEmailAsync(Guid.NewGuid(), "new@example.com");

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateEmailAsync_WhenEmailTakenByAnotherUser_ThrowsValidationException()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId);
        var otherUser = MakeUser(id: Guid.NewGuid(), email: "taken@example.com");
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.GetByEmailAsync("taken@example.com")).ReturnsAsync(otherUser);

        var act = async () => await _sut.UpdateEmailAsync(userId, "taken@example.com");

        await act.Should().ThrowAsync<ValidationException>().WithMessage("Email is already in use.");
    }

    // ── UpdatePasswordAsync ────────────────────────────────────────────

    [Fact]
    public async Task UpdatePasswordAsync_WithValidCredentials_UpdatesPassword()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId);
        user.hashedPassword = "old_hash";
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.VerifyPassword("OldPass@1", "old_hash")).Returns(true);
        _passwordHasherMock.Setup(p => p.HashPassword("NewPass@1")).Returns("new_hash");
        _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        await _sut.UpdatePasswordAsync(userId, "OldPass@1", "NewPass@1");

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.hashedPassword == "new_hash")), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = async () => await _sut.UpdatePasswordAsync(Guid.NewGuid(), "OldPass@1", "NewPass@1");

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdatePasswordAsync_WithWrongCurrentPassword_ThrowsValidationException()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId);
        user.hashedPassword = "old_hash";
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.VerifyPassword("WrongPass@1", "old_hash")).Returns(false);

        var act = async () => await _sut.UpdatePasswordAsync(userId, "WrongPass@1", "NewPass@1");

        await act.Should().ThrowAsync<ValidationException>().WithMessage("Current password is incorrect.");
    }

    // ── DeleteAccountAsync ─────────────────────────────────────────────

    [Fact]
    public async Task DeleteAccountAsync_WhenUserExists_DeletesUser()
    {
        var userId = Guid.NewGuid();
        var user = MakeUser(id: userId);
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

        await _sut.DeleteAccountAsync(userId);

        _userRepoMock.Verify(r => r.DeleteAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteAccountAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = async () => await _sut.DeleteAccountAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
