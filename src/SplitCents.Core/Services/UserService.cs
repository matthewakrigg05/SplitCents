namespace SplitCents.Core.Services;

using SplitCents.Core.DTOs;
using SplitCents.Core.Exceptions;
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
        
        // Normalise before uniqueness checks and storage so email matching is case-insensitive.
        var normalizedEmail = email.ToLowerInvariant();

        if (await _users.GetByEmailAsync(normalizedEmail) != null)
            throw new ValidationException("Email is already in use.");

        if (await _users.GetByDisplayNameAsync(displayName) != null)
            throw new ValidationException("Display name is already in use.");

        var hashedPassword = _passwordHasher.HashPassword(password);

        var userToReg = new User
        {
            id = Guid.NewGuid(),
            email = normalizedEmail,
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

    public async Task<UserResponse> LoginAsync(string email, string password)
    {
        var normalizedEmail = email.ToLowerInvariant();

        // Both failure cases return the same message to prevent user enumeration
        // (i.e. an attacker cannot tell whether the email exists).
        var user = await _users.GetByEmailAsync(normalizedEmail)
            ?? throw new ValidationException("Invalid email or password.");

        if (!_passwordHasher.VerifyPassword(password, user.hashedPassword))
            throw new ValidationException("Invalid email or password.");

        return new UserResponse
        {
            id = user.id,
            email = user.email,
            displayName = user.displayName,
            firstName = user.firstName,
            lastName = user.lastName
        };
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _users.GetByIdAsync(id);
    }

    public async Task UpdateDisplayNameAsync(Guid userId, string newDisplayName)
    {
        UserValidator.ValidateDisplayName(newDisplayName);

        var user = await _users.GetByIdAsync(userId)
            ?? throw new NotFoundException($"User with id '{userId}' was not found.");

        var existing = await _users.GetByDisplayNameAsync(newDisplayName);
        if (existing != null && existing.id != userId)
            throw new ValidationException("Display name is already in use.");

        user.displayName = newDisplayName;
        await _users.UpdateAsync(user);
    }

    public async Task UpdateEmailAsync(Guid userId, string newEmail)
    {
        UserValidator.ValidateEmail(newEmail);

        var user = await _users.GetByIdAsync(userId)
            ?? throw new NotFoundException($"User with id '{userId}' was not found.");

        var normalizedEmail = newEmail.ToLowerInvariant();

        var existing = await _users.GetByEmailAsync(normalizedEmail);
        if (existing != null && existing.id != userId)
            throw new ValidationException("Email is already in use.");

        user.email = normalizedEmail;
        await _users.UpdateAsync(user);
    }

    public async Task UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _users.GetByIdAsync(userId)
            ?? throw new NotFoundException($"User with id '{userId}' was not found.");

        if (!_passwordHasher.VerifyPassword(currentPassword, user.hashedPassword))
            throw new ValidationException("Current password is incorrect.");

        UserValidator.ValidatePassword(newPassword);

        user.hashedPassword = _passwordHasher.HashPassword(newPassword);
        await _users.UpdateAsync(user);
    }

    public async Task DeleteAccountAsync(Guid userId)
    {
        var user = await _users.GetByIdAsync(userId)
            ?? throw new NotFoundException($"User with id '{userId}' was not found.");

        await _users.DeleteAsync(user);
    }
}