
namespace FinTrack.Application.ExpenseCategories.Dtos;

public sealed record ExpenseCategoryDto(
    Guid Id,
    string Title,
    string Description,
    IReadOnlyList<ExpenseSubCategoryDto> SubCategories
);

public sealed record ExpenseSubCategoryDto(
    Guid Id,
    string Title,
    string Description
);