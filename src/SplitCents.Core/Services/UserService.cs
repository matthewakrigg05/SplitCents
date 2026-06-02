namespace SplitCents.Core.Services;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Models;

public class UserService : IUserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users)
    {
        _users = users;
    }

    public Task<User> RegisterAsync(string email, string password, string displayName)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    // Profile updates
    public Task UpdateDisplayNameAsync(Guid userId, string newDisplayName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateEmailAsync(Guid userId, string newEmail)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAccountAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}