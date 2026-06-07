namespace SplitCents.Core.Validators;

using System.Net.Mail;
using System.Text.RegularExpressions;
using SplitCents.Core.Exceptions;

public static class UserValidator
{
    public static void ValidateDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ValidationException("Display name cannot be empty.");

        if (displayName != displayName.Trim())
            throw new ValidationException("Display name cannot have leading or trailing whitespace.");

        if (displayName.Length < 2)
            throw new ValidationException("Display name must be at least 2 characters.");

        if (displayName.Length > 30)
            throw new ValidationException("Display name cannot exceed 30 characters.");

        if (!Regex.IsMatch(displayName, @"^[a-zA-Z0-9_\-]+$"))
            throw new ValidationException("Display name may only contain letters, digits, underscores, and hyphens.");
    }

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email cannot be empty.");

        try
        {
            var addr = new MailAddress(email);
            if (addr.Address != email)
                throw new ValidationException("Email is not a valid format.");
        }
        catch (FormatException)
        {
            throw new ValidationException("Email is not a valid format.");
        }
    }

    public static void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ValidationException("Password cannot be empty.");

        if (password.Any(char.IsWhiteSpace))
            throw new ValidationException("Password cannot contain whitespace.");

        if (password.Length < 8)
            throw new ValidationException("Password must be at least 8 characters.");

        if (password.Length > 128)
            throw new ValidationException("Password cannot exceed 128 characters.");

        if (!password.Any(char.IsUpper))
            throw new ValidationException("Password must contain at least one uppercase letter.");

        if (!password.Any(char.IsLower))
            throw new ValidationException("Password must contain at least one lowercase letter.");

        if (!password.Any(char.IsDigit))
            throw new ValidationException("Password must contain at least one digit.");

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            throw new ValidationException("Password must contain at least one special character.");
    }

}