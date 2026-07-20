namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class RecurringSubscriptionConfiguration : IEntityTypeConfiguration<RecurringSubscription>
{
    public void Configure(EntityTypeBuilder<RecurringSubscription> builder)
    {
        builder.HasKey(s => s.id);

        builder.Property(s => s.name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.provider)
            .HasMaxLength(150);

        builder.Property(s => s.notes)
            .HasMaxLength(1000);

        builder.Property(s => s.userId)
            .IsRequired();

        builder.HasIndex(s => new { s.userId, s.status });
    }
}
