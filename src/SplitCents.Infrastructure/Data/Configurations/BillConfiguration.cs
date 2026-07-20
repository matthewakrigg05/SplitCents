namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.id);

        builder.Property(b => b.name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.notes)
            .HasMaxLength(1000);

        builder.Property(b => b.userId)
            .IsRequired();

        builder.HasIndex(b => new { b.userId, b.isPaid });
    }
}
