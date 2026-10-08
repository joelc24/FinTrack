using FinTrack.Domain.Common;
using FinTrack.Domain.Incomes;
using FinTrack.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.Incomes;

public class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable("Incomes");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(i => i.Amount)
            .HasConversion(
                money => money.Amount,
                amount => Money.Create(amount).Value
            )
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.IncomeDate)
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne<Profile>()
            .WithMany()
            .HasForeignKey(i => i.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.ProfileId);
    }
}