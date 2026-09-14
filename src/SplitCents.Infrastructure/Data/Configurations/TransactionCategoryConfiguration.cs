namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
    public void Configure(EntityTypeBuilder<TransactionCategory> builder)
    {
        builder.HasKey(c => c.id);

        builder.Property(c => c.name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.description)
            .HasMaxLength(500);

        builder.HasDiscriminator<string>("categoryType")
            .HasValue<TransactionCategory>("TransactionCategory")
            .HasValue<UserTransactionCategory>("UserTransactionCategory");

        builder.HasData(
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000001"),
                name = "Income",
                description = "Salary, wages, benefits, and other incoming money.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000002"),
                name = "Housing",
                description = "Rent, mortgage, and household costs.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000003"),
                name = "Utilities",
                description = "Electricity, water, gas, internet, and phone bills.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000004"),
                name = "Groceries",
                description = "Food and household shopping.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000005"),
                name = "Transport",
                description = "Fuel, public transport, parking, and vehicle costs.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000006"),
                name = "Health",
                description = "Medical, dental, pharmacy, and wellness costs.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000007"),
                name = "Entertainment",
                description = "Dining out, hobbies, events, and subscriptions.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000008"),
                name = "Savings",
                description = "Savings contributions and investments.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000009"),
                name = "Debt",
                description = "Loan, credit card, and other debt payments.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TransactionCategory
            {
                id = new Guid("10000000-0000-0000-0000-000000000010"),
                name = "Other",
                description = "Transactions that do not fit another category.",
                createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                updatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}