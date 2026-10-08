
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;

namespace FinTrack.Application.Expenses.Queries.GetExpensesByProfileId;

public sealed record GetExpensesByProfileIdQuery(Guid ProfileId) : IQuery<IReadOnlyList<ExpenseDto>>;
