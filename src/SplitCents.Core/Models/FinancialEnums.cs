namespace SplitCents.Core.Models;

public enum BillFrequency
{
    Monthly = 0,
    Weekly = 1,
    Annual = 2
}

public enum SubscriptionStatus
{
    Active = 0,
    Cancelled = 1,
    Paused = 2
}

public enum TransactionType
{
    Income = 0,
    Expense = 1,
    Transfer = 2
}

public enum BudgetAllocationType
{
    FixedAmount = 0,
    PercentageOfIncome = 1
}
