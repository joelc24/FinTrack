
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;

namespace FinTrack.Application.Expenses.Commands.CreateExpense;

public sealed record CreateExpenseCommand(
    string Description, decimal TotalAmount, Guid ExpenseCategoryId,
    DateTime StartDate, DateTime FinishDate, Guid? AccountId,
    IReadOnlyList<Guid> SubCategoryIds) : ICommand<ExpenseDto>;