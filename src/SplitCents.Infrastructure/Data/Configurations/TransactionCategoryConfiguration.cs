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
    }
}