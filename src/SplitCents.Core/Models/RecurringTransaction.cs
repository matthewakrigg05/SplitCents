namespace SplitCents.Core.Models;

public class RecurringTransaction : Transaction
{
    public TransactionFrequency frequency { get; set; }
    public DateTime nextTransactionDate { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
    public SubscriptionStatus status { get; set; }
    public string provider { get; set; } = string.Empty;
}