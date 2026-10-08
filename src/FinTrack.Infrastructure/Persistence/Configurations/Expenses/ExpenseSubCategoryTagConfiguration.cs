

using FinTrack.Domain.ExpenseCategories;
using FinTrack.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.Expenses;

public class ExpenseSubCategoryTagConfiguration : IEntityTypeConfiguration<ExpenseSubCategoryTag>
{
    public void Configure(EntityTypeBuilder<ExpenseSubCategoryTag> builder)
    {
        builder.ToTable("ExpenseSubCategoryTags");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        // Cross-aggregate: FK real en BD, sin navegación de objeto — mismo patrón
        // que Expense → ExpenseCategory.
        builder.HasOne<ExpenseSubCategory>()
            .WithMany()
            .HasForeignKey(t => t.ExpenseSubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => new { t.ExpenseId, t.ExpenseSubCategoryId }).IsUnique();
    }
}