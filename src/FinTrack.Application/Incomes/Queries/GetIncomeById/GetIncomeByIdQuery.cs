

using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Incomes.Dtos;

namespace FinTrack.Application.Incomes.Queries.GetIncomeById;

public sealed record GetIncomeByIdQuery(Guid IncomeId) : IQuery<IncomeDto>;