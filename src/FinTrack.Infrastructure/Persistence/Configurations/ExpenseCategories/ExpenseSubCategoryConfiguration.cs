using FinTrack.Domain.ExpenseCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations.ExpenseCategories;

public class ExpenseSubCategoryConfiguration : IEntityTypeConfiguration<ExpenseSubCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseSubCategory> builder)
    {
        builder.ToTable("ExpenseSubCategories");

        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id)
            .ValueGeneratedNever();

        builder.Property(sc => sc.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(sc => sc.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(sc => sc.ExpenseCategoryId)
            .IsRequired();

        builder.HasIndex(sc => sc.ExpenseCategoryId);
    }
}
