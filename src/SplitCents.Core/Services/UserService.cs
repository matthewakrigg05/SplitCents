namespace SplitCents.Core.Services;

using System.ComponentModel.DataAnnotations;
using SplitCents.Core.DTOs;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Models;
using SplitCents.Core.Models.Security;
using SplitCents.Core.Validators;

public class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> RegisterAsync(string email, string password, string displayName, string? firstName, string? lastName)
    {
        UserValidator.ValidateEmail(email);
        UserValidator.ValidateDisplayName(displayName);
        UserValidator.ValidatePassword(password);
        
        if (await _users.GetByEmailAsync(email) != null)
            throw new ValidationException("Email is already in use.");

        if (await _users.GetByDisplayNameAsync(displayName) != null)
            throw new ValidationException("Display name is already in use.");
        
        var hashedPassword = _passwordHasher.HashPassword(password);

        User userToReg = new User
            {
                id = Guid.NewGuid(),
                email = email.ToLowerInvariant(), 
                hashedPassword = hashedPassword, 
                displayName = displayName, 
                firstName = firstName ?? string.Empty, 
                lastName = lastName ?? string.Empty
            };

        await _users.AddAsync(userToReg);
        
        return new UserResponse
            {
                id = userToReg.id,
                email = userToReg.email,
                displayName = userToReg.displayName,
                firstName = userToReg.firstName,
                lastName = userToReg.lastName
            };
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