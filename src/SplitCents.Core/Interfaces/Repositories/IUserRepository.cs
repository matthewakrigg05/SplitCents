namespace SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Models;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByDisplayNameAsync(string displayName);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}