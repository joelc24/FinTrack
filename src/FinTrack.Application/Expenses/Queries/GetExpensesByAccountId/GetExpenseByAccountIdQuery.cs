
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;

namespace FinTrack.Application.Expenses.Queries.GetExpensesByAccountId;

public sealed record GetExpenseByAccountIdQuery(Guid AccountId) : IQuery<IReadOnlyList<ExpenseDto>>;