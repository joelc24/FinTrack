
using FinTrack.Domain.ExpenseCategories;

namespace FinTrack.Application.ExpenseCategories.Dtos;

public static class ExpenseCategoriesMappingExtensions
{
    public static ExpenseCategoryDto ToDto(this ExpenseCategory expenseCategory)
    {
        return new(
            expenseCategory.Id,
            expenseCategory.Title,
            expenseCategory.Description,
            expenseCategory.SubCategories.Select(sc => sc.ToDto()).ToList()
        );
    } 

    public static ExpenseSubCategoryDto ToDto(this ExpenseSubCategory expenseSubCategory)
    {
        return new(
            expenseSubCategory.Id,
            expenseSubCategory.Title,
            expenseSubCategory.Description
        );
    }
}