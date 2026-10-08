using FinTrack.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.Accounts;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(a => a.Email)
            .IsUnique();
        
    }
}