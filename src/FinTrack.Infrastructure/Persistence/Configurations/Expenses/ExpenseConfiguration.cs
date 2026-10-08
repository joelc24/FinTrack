using FinTrack.Domain.Accounts;
using FinTrack.Domain.Common;
using FinTrack.Domain.ExpenseCategories;
using FinTrack.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.Expenses;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.TotalAmount)
            .HasConversion(
                money => money.Amount,
                amount => Money.Create(amount).Value
            )
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.ComplexProperty(e => e.DateRange, dateRangeBuilder =>
        {
            dateRangeBuilder.Property(d => d.StartDate)
                .HasColumnName("StartDate")
                .HasColumnType("datetime2")
                .IsRequired();

            dateRangeBuilder.Property(d => d.FinishDate)
                .HasColumnName("FinishDate")
                .HasColumnType("datetime2")
                .IsRequired();
        });

        builder.HasOne<ExpenseCategory>()
            .WithMany()
            .HasForeignKey(e => e.ExpenseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.Allocations)
            .WithOne()
            .HasForeignKey(a => a.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.SubCategoryTags)
            .WithOne()
            .HasForeignKey(t => t.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasIndex(e => e.ExpenseCategoryId);
        builder.HasIndex(e => e.AccountId);

        builder.Metadata.FindNavigation(nameof(Expense.Allocations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Metadata.FindNavigation(nameof(Expense.SubCategoryTags))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
