namespace SplitCents.Core.Tests;

using SplitCents.Core.Exceptions;
using SplitCents.Core.Validators;
using FluentAssertions;
using Xunit;

public class UserValidatorTests
{
    // Validate Email

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

    // ValidateDisplayName

    [Fact]
    public void ValidateDisplayName_WithValidName_DoesNotThrow()
    {
        var act = () => UserValidator.ValidateDisplayName("cool_user-1");

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateDisplayName_WithNullOrWhitespace_ThrowsValidationException(string? displayName)
    {
        #pragma warning disable CS8604
        var act = () => UserValidator.ValidateDisplayName(displayName);
        #pragma warning restore CS8604

        act.Should().Throw<ValidationException>()
           .WithMessage("Display name cannot be empty.");
    }

    [Theory]
    [InlineData(" leading")]
    [InlineData("trailing ")]
    [InlineData(" both ")]
    public void ValidateDisplayName_WithLeadingOrTrailingWhitespace_ThrowsValidationException(string displayName)
    {
        var act = () => UserValidator.ValidateDisplayName(displayName);

        act.Should().Throw<ValidationException>()
           .WithMessage("Display name cannot have leading or trailing whitespace.");
    }

    [Fact]
    public void ValidateDisplayName_WithSingleCharacter_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidateDisplayName("a");

        act.Should().Throw<ValidationException>()
           .WithMessage("Display name must be at least 2 characters.");
    }

    [Fact]
    public void ValidateDisplayName_WithExactMinLength_DoesNotThrow()
    {
        var act = () => UserValidator.ValidateDisplayName("ab");

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateDisplayName_WithExactMaxLength_DoesNotThrow()
    {
        var act = () => UserValidator.ValidateDisplayName(new string('a', 30));

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateDisplayName_ExceedingMaxLength_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidateDisplayName(new string('a', 31));

        act.Should().Throw<ValidationException>()
           .WithMessage("Display name cannot exceed 30 characters.");
    }

    [Theory]
    [InlineData("hello world")]
    [InlineData("user@name")]
    [InlineData("name!")]
    [InlineData("näme")]
    public void ValidateDisplayName_WithDisallowedCharacters_ThrowsValidationException(string displayName)
    {
        var act = () => UserValidator.ValidateDisplayName(displayName);

        act.Should().Throw<ValidationException>()
           .WithMessage("Display name may only contain letters, digits, underscores, and hyphens.");
    }

    // ValidatePassword

    [Fact]
    public void ValidatePassword_WithValidPassword_DoesNotThrow()
    {
        var act = () => UserValidator.ValidatePassword("Secure@99");

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ValidatePassword_WithNullOrEmpty_ThrowsValidationException(string? password)
    {
        #pragma warning disable CS8604
        var act = () => UserValidator.ValidatePassword(password);
        #pragma warning restore CS8604

        act.Should().Throw<ValidationException>()
           .WithMessage("Password cannot be empty.");
    }

    [Theory]
    [InlineData("Pass @99")]
    [InlineData("Pass\t@99")]
    [InlineData("Pass\n@99")]
    public void ValidatePassword_WithWhitespace_ThrowsValidationException(string password)
    {
        var act = () => UserValidator.ValidatePassword(password);

        act.Should().Throw<ValidationException>()
           .WithMessage("Password cannot contain whitespace.");
    }

    [Fact]
    public void ValidatePassword_TooShort_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidatePassword("Ab1!");

        act.Should().Throw<ValidationException>()
           .WithMessage("Password must be at least 8 characters.");
    }

    [Fact]
    public void ValidatePassword_WithExactMinLength_DoesNotThrow()
    {
        var act = () => UserValidator.ValidatePassword("Abcde1!x");

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidatePassword_ExceedingMaxLength_ThrowsValidationException()
    {
        var longPassword = "Aa1!" + new string('x', 125);

        var act = () => UserValidator.ValidatePassword(longPassword);

        act.Should().Throw<ValidationException>()
           .WithMessage("Password cannot exceed 128 characters.");
    }

    [Fact]
    public void ValidatePassword_WithExactMaxLength_DoesNotThrow()
    {
        var longPassword = "Aa1!" + new string('x', 124);

        var act = () => UserValidator.ValidatePassword(longPassword);

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidatePassword_MissingUppercase_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidatePassword("secure@99");

        act.Should().Throw<ValidationException>()
           .WithMessage("Password must contain at least one uppercase letter.");
    }

    [Fact]
    public void ValidatePassword_MissingLowercase_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidatePassword("SECURE@99");

        act.Should().Throw<ValidationException>()
           .WithMessage("Password must contain at least one lowercase letter.");
    }

    [Fact]
    public void ValidatePassword_MissingDigit_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidatePassword("Secure@xx");

        act.Should().Throw<ValidationException>()
           .WithMessage("Password must contain at least one digit.");
    }

    [Fact]
    public void ValidatePassword_MissingSpecialCharacter_ThrowsValidationException()
    {
        var act = () => UserValidator.ValidatePassword("Secure999");

        act.Should().Throw<ValidationException>()
           .WithMessage("Password must contain at least one special character.");
    }
}