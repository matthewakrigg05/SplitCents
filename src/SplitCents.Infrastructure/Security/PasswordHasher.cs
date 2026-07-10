namespace SplitCents.Infrastructure.Security;

using SplitCents.Core.Models.Security;

public class PasswordHasher : IPasswordHasher
{
    // BCrypt handles salting internally; no manual salt management is needed.
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}
