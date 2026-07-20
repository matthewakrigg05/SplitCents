namespace SplitCents.Core.Models;

public class Transaction
{
    public Guid id { get; set; }
    public Guid userId { get; set; }
    public string description { get; set; } = string.Empty;
    public decimal amount { get; set; }
    public TransactionType type { get; set; }
    public DateTime transactionDate { get; set; }
    public string category { get; set; } = string.Empty;
    public string notes { get; set; } = string.Empty;
    public bool isPlanned { get; set; }
    public Guid? billId { get; set; }
    public Guid? subscriptionId { get; set; }
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public DateTime updatedAt { get; set; } = DateTime.UtcNow;
}
