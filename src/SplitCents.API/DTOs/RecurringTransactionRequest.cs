namespace SplitCents.API.DTOs;

using SplitCents.Core.Models;

public class RecurringTransactionRequest
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionFrequency Frequency { get; set; }
    public DateTime NextTransactionDate { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Notes { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Provider { get; set; }
}