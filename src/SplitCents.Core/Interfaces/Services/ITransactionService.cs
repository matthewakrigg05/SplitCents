namespace SplitCents.Core.Interfaces.Services;

using SplitCents.Core.Models;

public interface ITransactionService
{
    Task<RecurringTransaction> CreateRecurringAsync(
        Guid userId,
        string description,
        decimal amount,
        TransactionFrequency frequency,
        DateTime nextTransactionDate,
        Guid? categoryId = null,
        string? notes = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? provider = null);

    Task<IReadOnlyList<RecurringTransaction>> GetRecurringAsync(Guid userId);
    Task<RecurringTransaction> GetRecurringByIdAsync(Guid userId, Guid transactionId);

    Task UpdateRecurringAsync(
        Guid userId,
        Guid transactionId,
        string description,
        decimal amount,
        TransactionFrequency frequency,
        DateTime nextTransactionDate,
        Guid? categoryId = null,
        string? notes = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? provider = null);

    Task DeleteRecurringAsync(Guid userId, Guid transactionId);
    Task MarkRecurringPaidAsync(Guid userId, Guid transactionId, DateTime? paidOn = null);
    Task MarkRecurringUnpaidAsync(Guid userId, Guid transactionId);
    Task<IReadOnlyList<RecurringTransaction>> FindUpcomingRecurringAsync(Guid userId, DateTime from, int daysAhead);
}