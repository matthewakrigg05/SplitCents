namespace SplitCents.Core.Models;

public enum TransactionFrequency
{
    Monthly = 0,
    Weekly = 1,
    Annual = 2, 
    BiWeekly = 3
}

public enum SubscriptionStatus
{
    Active = 0,
    Cancelled = 1,
    Paused = 2,
    Skipped = 3
}

public enum TransactionType
{
    Income = 0,
    Expense = 1,
    Transfer = 2
}
