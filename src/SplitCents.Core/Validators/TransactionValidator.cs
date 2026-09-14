namespace SplitCents.Core.Validators;

using SplitCents.Core.Exceptions;
using SplitCents.Core.Models;

public static class TransactionValidator
{
    public static void ValidateRecurring(
        string description,
        decimal amount,
        TransactionFrequency frequency,
        DateTime nextTransactionDate,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Transaction description cannot be empty.");

        if (description != description.Trim())
            throw new ValidationException("Transaction description cannot have leading or trailing whitespace.");

        if (description.Length > 250)
            throw new ValidationException("Transaction description cannot exceed 250 characters.");

        if (amount <= 0)
            throw new ValidationException("Transaction amount must be greater than zero.");

        if (!Enum.IsDefined(frequency))
            throw new ValidationException("Transaction frequency is invalid.");

        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
            throw new ValidationException("Transaction end date cannot be before its start date.");

        if (startDate.HasValue && nextTransactionDate < startDate)
            throw new ValidationException("Next transaction date cannot be before its start date.");

        if (endDate.HasValue && nextTransactionDate > endDate)
            throw new ValidationException("Next transaction date cannot be after its end date.");
    }
}