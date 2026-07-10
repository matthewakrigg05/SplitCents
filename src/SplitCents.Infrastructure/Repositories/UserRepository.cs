namespace SplitCents.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Models;
using SplitCents.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly SplitCentsDbContext _context;

    public UserRepository(SplitCentsDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.email == email);

    public async Task<User?> GetByDisplayNameAsync(string displayName) =>
        await _context.Users.FirstOrDefaultAsync(u => u.displayName == displayName);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
