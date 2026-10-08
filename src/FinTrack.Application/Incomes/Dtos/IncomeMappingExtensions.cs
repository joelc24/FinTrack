
using FinTrack.Domain.Incomes;

namespace FinTrack.Application.Incomes.Dtos;

public static class IncomeMappingExtensions
{
    public static IncomeDto ToDto(this Income income) =>
        new(income.Id, income.Description, income.Amount.Amount, income.IncomeDate, income.ProfileId);
}