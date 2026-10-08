using FinTrack.Domain.Accounts;
using FinTrack.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.Profiles;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(p => p.LastName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.UserName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(p => p.PasswordHash)
            .HasMaxLength(255);

        builder.Ignore(p => p.IsProtected);       

        builder.Property(p => p.IsAdmin)
            .HasColumnType("bit")
            .IsRequired();
        
        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.AccountId);

    }
}