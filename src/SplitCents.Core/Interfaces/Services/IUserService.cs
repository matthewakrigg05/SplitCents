namespace SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Models;

public interface IUserService
{
    // Reg & Retrieval
    Task<User> RegisterAsync(string email, string password, string displayName);
    Task<User?> GetUserByIdAsync(Guid id);

    // Profile updates
    Task UpdateDisplayNameAsync(Guid userId, string newDisplayName);
    Task UpdateEmailAsync(Guid userId, string newEmail);
    Task UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);

    // Account management
    Task DeleteAccountAsync(Guid userId);
}