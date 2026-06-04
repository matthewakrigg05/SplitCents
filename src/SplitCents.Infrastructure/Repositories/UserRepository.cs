namespace SplitCents.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Models;

public class UserRepository : IUserRepository 
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByDisplayNameAsync(string displayName)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(User user)
    {
        throw new NotImplementedException();
    }
}