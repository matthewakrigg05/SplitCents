namespace SplitCents.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Models;
using SplitCents.Infrastructure.Data;

public class TransactionRepository : ITransactionRepository
{
    private readonly SplitCentsDbContext _context;

    public TransactionRepository(SplitCentsDbContext context)
    {
        _context = context;
    }

    public async Task AddRecurringAsync(RecurringTransaction transaction)
    {
        await _context.RecurringTransactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<RecurringTransaction>> GetRecurringAsync(Guid userId) =>
        await _context.RecurringTransactions
            .Where(transaction => transaction.userId == userId)
            .OrderBy(transaction => transaction.nextTransactionDate)
            .ToListAsync();

    public async Task<RecurringTransaction?> GetRecurringByIdAsync(Guid userId, Guid transactionId) =>
        await _context.RecurringTransactions
            .FirstOrDefaultAsync(transaction =>
                transaction.userId == userId && transaction.id == transactionId);

    public async Task<IReadOnlyList<RecurringTransaction>> GetUpcomingRecurringAsync(
        Guid userId,
        DateTime from,
        DateTime through) =>
        await _context.RecurringTransactions
            .Where(transaction =>
                transaction.userId == userId &&
                transaction.nextTransactionDate >= from &&
                transaction.nextTransactionDate <= through)
            .OrderBy(transaction => transaction.nextTransactionDate)
            .ToListAsync();

    public async Task UpdateRecurringAsync(RecurringTransaction transaction)
    {
        _context.RecurringTransactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRecurringAsync(RecurringTransaction transaction)
    {
        _context.RecurringTransactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}