namespace SplitCents.Core.Models
{
    public class Budget
    {
        public Guid id { get; set; }
        public Guid userId { get; set; }
        public Guid? categoryId { get; set; }
        public string name { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }
}
