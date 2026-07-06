namespace SplitCents.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitCents.Core.Models;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.id);

        builder.Property(u => u.email)
            .IsRequired()
            .HasMaxLength(254);
        builder.HasIndex(u => u.email)
            .IsUnique();

        builder.Property(u => u.displayName)
            .IsRequired()
            .HasMaxLength(30);
        builder.HasIndex(u => u.displayName)
            .IsUnique();

        builder.Property(u => u.hashedPassword)
            .IsRequired();

        builder.Property(u => u.firstName)
            .HasMaxLength(100)
            .HasDefaultValue(string.Empty);

        builder.Property(u => u.lastName)
            .HasMaxLength(100)
            .HasDefaultValue(string.Empty);
    }
}
