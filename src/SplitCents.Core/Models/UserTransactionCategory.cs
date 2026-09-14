namespace SplitCents.Core.Models;

public class UserTransactionCategory : TransactionCategory
{
    public Guid userId { get; set; }
}