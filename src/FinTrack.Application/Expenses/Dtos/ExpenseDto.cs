
namespace FinTrack.Application.Expenses.Dtos;

public sealed record ExpenseDto(
    Guid Id,
    string Description,
    decimal TotalAmount,
    Guid ExpenseCategoryId,
    Guid? AccountId,
    Guid CreatedBy,
    DateTime StartDate,
    DateTime FinishDate,
    IReadOnlyList<ExpenseAllocationDto> Allocations,
    IReadOnlyList<Guid> SubCategoryIds);

public sealed record ExpenseAllocationDto(
    Guid Id,
    Guid ProfileId,
    decimal AssignedAmount,
    DateOnly? PaidAt);