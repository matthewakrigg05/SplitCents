namespace SplitCents.Core.Models;

public class Bill
{
    public Guid id { get; set; }
    public Guid userId { get; set; }
    public string name { get; set; } = string.Empty;
    public decimal amount { get; set; }
    public BillFrequency frequency { get; set; }
    public DateTime dueDate { get; set; }
    public bool isPaid { get; set; }
    public DateTime? paidOn { get; set; }
    public string notes { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public DateTime updatedAt { get; set; } = DateTime.UtcNow;
}
