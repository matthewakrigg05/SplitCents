namespace SplitCents.Core.Models;

public class UserCategory : Category
{
    public Guid userCategoryId { get; set; }
    public Guid userId { get; set; }
}