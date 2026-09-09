namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.id);

        builder.Property(t => t.description)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(t => t.amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.notes)
            .HasMaxLength(1000);

        builder.Property(t => t.userId)
            .IsRequired();

        builder.HasIndex(t => new { t.userId, t.transactionDate });
        builder.HasIndex(t => t.categoryId);

        builder.HasOne<TransactionCategory>()
            .WithMany()
            .HasForeignKey(t => t.categoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasDiscriminator<string>("transactionType")
            .HasValue<Transaction>("Transaction")
            .HasValue<RecurringTransaction>("RecurringTransaction");
    }
}
