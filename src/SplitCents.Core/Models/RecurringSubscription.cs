namespace SplitCents.Core.Models;

public class RecurringSubscription
{
    public Guid id { get; set; }
    public Guid userId { get; set; }
    public string name { get; set; } = string.Empty;
    public decimal amount { get; set; }
    public BillFrequency frequency { get; set; }
    public DateTime nextBillingDate { get; set; }
    public DateTime? startDate { get; set; }
    public SubscriptionStatus status { get; set; }
    public string provider { get; set; } = string.Empty;
    public string notes { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public DateTime updatedAt { get; set; } = DateTime.UtcNow;
}
