
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Expenses.Commands.DeleteExpense;

public sealed record DeleteExpenseCommand(Guid ExpenseId) : ICommand;