
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;

namespace FinTrack.Application.Expenses.Queries.GetExpenseById;

public sealed record GetExpenseByIdQuery(Guid ExpenseId) : IQuery<ExpenseDto>;