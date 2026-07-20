namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class BudgetCategoryConfiguration : IEntityTypeConfiguration<BudgetCategory>
{
    public void Configure(EntityTypeBuilder<BudgetCategory> builder)
    {
        builder.HasKey(b => b.id);

        builder.Property(b => b.name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.percentageOfIncome)
            .HasColumnType("decimal(5,2)");

        builder.Property(b => b.userId)
            .IsRequired();

        builder.HasIndex(b => new { b.userId, b.month, b.isActive });
    }
}
