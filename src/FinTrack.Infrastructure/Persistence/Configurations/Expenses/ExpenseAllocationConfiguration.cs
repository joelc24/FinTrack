using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.Expenses;

public class ExpenseAllocationConfiguration : IEntityTypeConfiguration<ExpenseAllocation>
{
    public void Configure(EntityTypeBuilder<ExpenseAllocation> builder)
    {
        builder.ToTable("ExpenseAllocations");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.ProfileId)
            .IsRequired();

        builder.Property(a => a.ExpenseId)
            .IsRequired();

        builder.Property(a => a.AssignedAmount)
            .HasConversion(
                money => money.Amount,
                amount => Money.Create(amount).Value
            )
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(a => a.PaidAt)
            .HasColumnType("date");

        builder.HasIndex(a => a.ProfileId);
        builder.HasIndex(a => a.ExpenseId);
    }
}
