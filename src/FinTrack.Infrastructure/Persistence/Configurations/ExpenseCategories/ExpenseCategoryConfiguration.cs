using FinTrack.Domain.ExpenseCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.ExpenseCategories;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.ToTable("ExpenseCategories");

        builder.HasKey(ec => ec.Id);

        builder.Property(ec => ec.Id)
            .ValueGeneratedNever();

        builder.Property(ec => ec.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(ec => ec.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasMany(ec => ec.SubCategories)
            .WithOne()
            .HasForeignKey(sc => sc.ExpenseCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(ExpenseCategory.SubCategories))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
