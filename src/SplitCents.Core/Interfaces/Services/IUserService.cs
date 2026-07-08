namespace SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Models;
using SplitCents.Core.DTOs;

public interface IUserService
{
    // Reg & Retrieval
    Task<UserResponse> RegisterAsync(string email, string password, string displayName, string? firstName, string? lastName);
    Task<UserResponse> LoginAsync(string email, string password);
    Task<User?> GetUserByIdAsync(Guid id);

    // Profile updates
    Task UpdateDisplayNameAsync(Guid userId, string newDisplayName);
    Task UpdateEmailAsync(Guid userId, string newEmail);
    Task UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);

    // Account management
    Task DeleteAccountAsync(Guid userId);
}