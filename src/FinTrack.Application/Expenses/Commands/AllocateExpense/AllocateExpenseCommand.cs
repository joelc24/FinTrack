
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Expenses.Commands.AllocateExpense;

public sealed record AllocateExpenseCommand(
    Guid ExpenseId, Guid ProfileId, decimal Amount, DateOnly? PaidAt) : ICommand;