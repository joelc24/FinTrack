namespace FinTrack.WebApi.Contracts;

public sealed record CreateExpenseRequest(
    string Description, decimal TotalAmount, Guid ExpenseCategoryId,
    DateTime StartDate, DateTime FinishDate, Guid? AccountId,
    IReadOnlyList<Guid> SubCategoryIds);

public sealed record UpdateExpenseRequest(
    string Description, decimal TotalAmount, Guid ExpenseCategoryId,
    DateTime StartDate, DateTime FinishDate,
    IReadOnlyList<Guid> SubCategoryIds);

public sealed record AllocateExpenseRequest(Guid ProfileId, decimal Amount, DateOnly? PaidAt);