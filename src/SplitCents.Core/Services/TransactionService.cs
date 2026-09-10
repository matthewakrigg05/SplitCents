namespace SplitCents.Core.Services;

using SplitCents.Core.Exceptions;
using SplitCents.Core.Interfaces.Repositories;
using SplitCents.Core.Interfaces.Services;
using SplitCents.Core.Models;
using SplitCents.Core.Validators;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactions;

    public TransactionService(ITransactionRepository transactions)
    {
        _transactions = transactions;
    }

    public async Task<RecurringTransaction> CreateRecurringAsync(
        Guid userId,
        string description,
        decimal amount,
        TransactionFrequency frequency,
        DateTime nextTransactionDate,
        Guid? categoryId = null,
        string? notes = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? provider = null)
    {
        TransactionValidator.ValidateRecurring(description, amount, frequency, nextTransactionDate, startDate, endDate);

        var transaction = new RecurringTransaction
        {
            id = Guid.NewGuid(),
            userId = userId,
            description = description,
            amount = amount,
            type = TransactionType.Expense,
            transactionDate = nextTransactionDate,
            categoryId = categoryId,
            notes = notes ?? string.Empty,
            frequency = frequency,
            nextTransactionDate = nextTransactionDate,
            startDate = startDate,
            endDate = endDate,
            status = SubscriptionStatus.Active,
            provider = provider ?? string.Empty
        };

        await _transactions.AddRecurringAsync(transaction);
        return transaction;
    }

    public Task<IReadOnlyList<RecurringTransaction>> GetRecurringAsync(Guid userId) =>
        _transactions.GetRecurringAsync(userId);

    public async Task<RecurringTransaction> GetRecurringByIdAsync(Guid userId, Guid transactionId) =>
        await GetOwnedRecurringAsync(userId, transactionId);

    public async Task UpdateRecurringAsync(
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
        string? provider = null)
    {
        TransactionValidator.ValidateRecurring(description, amount, frequency, nextTransactionDate, startDate, endDate);
        var transaction = await GetOwnedRecurringAsync(userId, transactionId);

        transaction.description = description;
        transaction.amount = amount;
        transaction.transactionDate = nextTransactionDate;
        transaction.categoryId = categoryId;
        transaction.notes = notes ?? string.Empty;
        transaction.frequency = frequency;
        transaction.nextTransactionDate = nextTransactionDate;
        transaction.startDate = startDate;
        transaction.endDate = endDate;
        transaction.provider = provider ?? string.Empty;
        transaction.updatedAt = DateTime.UtcNow;

        await _transactions.UpdateRecurringAsync(transaction);
    }

    public async Task DeleteRecurringAsync(Guid userId, Guid transactionId)
    {
        var transaction = await GetOwnedRecurringAsync(userId, transactionId);
        await _transactions.DeleteRecurringAsync(transaction);
    }

    public async Task MarkRecurringPaidAsync(Guid userId, Guid transactionId, DateTime? paidOn = null)
    {
        var transaction = await GetOwnedRecurringAsync(userId, transactionId);
        transaction.isPaid = true;
        transaction.paidOn = paidOn ?? DateTime.UtcNow;
        transaction.updatedAt = DateTime.UtcNow;
        await _transactions.UpdateRecurringAsync(transaction);
    }

    public async Task MarkRecurringUnpaidAsync(Guid userId, Guid transactionId)
    {
        var transaction = await GetOwnedRecurringAsync(userId, transactionId);
        transaction.isPaid = false;
        transaction.paidOn = null;
        transaction.updatedAt = DateTime.UtcNow;
        await _transactions.UpdateRecurringAsync(transaction);
    }

    public async Task<IReadOnlyList<RecurringTransaction>> FindUpcomingRecurringAsync(Guid userId, DateTime from, int daysAhead)
    {
        if (daysAhead < 0)
            throw new ValidationException("Upcoming transaction days cannot be negative.");

        return await _transactions.GetUpcomingRecurringAsync(userId, from, from.AddDays(daysAhead));
    }

    private async Task<RecurringTransaction> GetOwnedRecurringAsync(Guid userId, Guid transactionId)
    {
        return await _transactions.GetRecurringByIdAsync(userId, transactionId)
            ?? throw new NotFoundException($"Transaction with id '{transactionId}' was not found.");
    }
}