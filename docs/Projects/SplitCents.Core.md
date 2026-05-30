# SplitCents.Core

## Purpose
The domain heart of the application. Contains everything that describes *what* SplitCents does, with zero knowledge of *how* it is stored or delivered. Has no dependencies on ASP.NET, Entity Framework, or any other framework.

## Scope

### Models
Plain C# classes representing the domain. Current models:
- `Transaction` — a single financial event (income, expense, or transfer)
- `Account` — a wallet, bank account, or credit card
- `Category` — a spending/income label attached to transactions
- `Budget` — a spending limit for a category over a period
- `User` — an account holder

### Enums
- `TransactionType`: Income / Expense / Transfer
- `AccountType`: Cash / Bank / CreditCard / Savings
- `BudgetPeriod`: Daily / Weekly / Monthly / Yearly
- `SyncStatus`: Clean / Dirty / PendingUpload / Conflicted

### Interfaces
Contracts that Infrastructure must implement. Examples:
- `ITransactionRepository`, `IAccountRepository`, `IBudgetRepository`
- `ISyncService`

### Domain Logic
Pure business rules with no I/O:
- Budget period calculations (has a budget been exceeded?)
- Split/share calculations between users
- Sync conflict resolution rules

## What Does NOT Belong Here
- Database queries or EF Core types
- HTTP request/response types
- Any `using Microsoft.AspNetCore.*` or `using Microsoft.EntityFrameworkCore.*`
