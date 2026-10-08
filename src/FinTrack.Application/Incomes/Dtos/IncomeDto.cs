
namespace FinTrack.Application.Incomes.Dtos;

public sealed record IncomeDto(Guid Id, string Description, decimal Amount, DateOnly IncomeDate, Guid ProfileId);