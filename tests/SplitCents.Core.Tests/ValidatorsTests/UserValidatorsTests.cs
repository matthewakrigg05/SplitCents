namespace SplitCents.Core.Tests;

using SplitCents.Core.Exceptions;
using SplitCents.Core.Validators;
using FluentAssertions;
using Xunit;

public class UserValidatorTests
{
    // ── ValidateEmail ────────────────────────────────────────────────────────

    [Fact]
    public void ValidateEmail_WithValidEmail_DoesNotThrow()
    {
        var act = () => UserValidator.ValidateEmail("user@example.com");

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateEmail_WithNullOrWhitespace_ThrowsValidationException(string? email)
    {
        #pragma warning disable CS8604 // Possible null reference argument.
        var act = () => UserValidator.ValidateEmail(email);
        #pragma warning restore CS8604 // Possible null reference argument.

        act.Should().Throw<ValidationException>()
           .WithMessage("Email cannot be empty.");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@")]
    [InlineData("@nodomain.com")]
    [InlineData("spaces in@email.com")]
    public void ValidateEmail_WithInvalidFormat_ThrowsValidationException(string email)
    {
        var act = () => UserValidator.ValidateEmail(email);

        act.Should().Throw<ValidationException>()
           .WithMessage("Email is not a valid format.");
    }
}