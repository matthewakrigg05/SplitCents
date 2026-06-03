namespace SplitCents.Core.Validators;

using System.Net.Mail;
using SplitCents.Core.Exceptions;

public static class UserValidator
{
    public static void ValidateDisplayName(string displayName)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

}