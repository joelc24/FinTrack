

namespace FinTrack.Domain.Expenses;

public sealed class ExpenseSubCategoryTag
{
    public Guid Id { get; private set; }
    public Guid ExpenseId { get; private set; }
    public Guid ExpenseSubCategoryId { get; private set; }

    private ExpenseSubCategoryTag() { }

    internal static ExpenseSubCategoryTag Create(Guid expenseId, Guid expenseSubCategoryId) =>
        new() { Id = Guid.CreateVersion7(), ExpenseId = expenseId, ExpenseSubCategoryId = expenseSubCategoryId };
}