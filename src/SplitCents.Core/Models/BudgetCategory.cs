using System;

namespace SplitCents.Core.Models
{
    public class BudgetCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
