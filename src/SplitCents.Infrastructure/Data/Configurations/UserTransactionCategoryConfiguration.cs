namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class UserTransactionCategoryConfiguration : IEntityTypeConfiguration<UserTransactionCategory>
{
    public void Configure(EntityTypeBuilder<UserTransactionCategory> builder)
    {
        builder.Property(c => c.userId)
            .IsRequired();

        builder.HasIndex(c => new { c.userId, c.name })
            .IsUnique();
    }
}