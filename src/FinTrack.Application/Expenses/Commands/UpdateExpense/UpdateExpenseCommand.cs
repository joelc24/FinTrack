
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Expenses.Commands.UpdateExpense;

public sealed record UpdateExpenseCommand(
    Guid ExpenseId, string Description, decimal TotalAmount, Guid ExpenseCategoryId,
    DateTime StartDate, DateTime FinishDate, IReadOnlyList<Guid> SubCategoryIds) : ICommand;