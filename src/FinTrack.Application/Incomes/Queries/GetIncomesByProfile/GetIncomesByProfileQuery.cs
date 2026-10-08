

using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Incomes.Dtos;

namespace FinTrack.Application.Incomes.Queries.GetIncomesByProfile;

public sealed record GetIncomesByProfileQuery(Guid ProfileId) : IQuery<IReadOnlyList<IncomeDto>>;