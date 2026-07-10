namespace SplitCents.Infrastructure.Tests.Unit;

using SplitCents.Infrastructure.Security;
using FluentAssertions;
using Xunit;

public class PasswordHasherTests
{
    private readonly PasswordHasher _sut = new();

    [Fact]
    public void HashPassword_ReturnsDifferentStringThanInput()
    {
        var hash = _sut.HashPassword("MyPass@99");
        hash.Should().NotBe("MyPass@99");
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
    {
        var hash = _sut.HashPassword("MyPass@99");
        _sut.VerifyPassword("MyPass@99", hash).Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithWrongPassword_ReturnsFalse()
    {
        var hash = _sut.HashPassword("MyPass@99");
        _sut.VerifyPassword("WrongPass@99", hash).Should().BeFalse();
    }

    [Fact]
    public void HashPassword_TwiceForSamePassword_ProducesDifferentHashes()
    {
        // BCrypt generates a new random salt on every call, so identical passwords never produce the same hash.
        var hash1 = _sut.HashPassword("MyPass@99");
        var hash2 = _sut.HashPassword("MyPass@99");
        hash1.Should().NotBe(hash2);
    }
}
