namespace SplitCents.Core.Interfaces.Repositories;

using SplitCents.Core.Models;

public interface ITransactionRepository
{
    Task AddRecurringAsync(RecurringTransaction transaction);
    Task<IReadOnlyList<RecurringTransaction>> GetRecurringAsync(Guid userId);
    Task<RecurringTransaction?> GetRecurringByIdAsync(Guid userId, Guid transactionId);
    Task<IReadOnlyList<RecurringTransaction>> GetUpcomingRecurringAsync(Guid userId, DateTime from, DateTime through);
    Task UpdateRecurringAsync(RecurringTransaction transaction);
    Task DeleteRecurringAsync(RecurringTransaction transaction);
}