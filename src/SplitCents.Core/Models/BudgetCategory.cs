namespace SplitCents.Core.Models;

public class BudgetCategory
{
    public Guid id { get; set; }
    public Guid userId { get; set; }
    public string name { get; set; } = string.Empty;
    public BudgetAllocationType allocationType { get; set; }
    public decimal amount { get; set; }
    public decimal? percentageOfIncome { get; set; }
    public DateTime month { get; set; }
    public bool isActive { get; set; }
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public DateTime updatedAt { get; set; } = DateTime.UtcNow;
}
