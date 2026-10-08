
namespace FinTrack.WebApi.Contracts;

public sealed record CreateIncomeRequest(string Description, decimal Amount, DateOnly IncomeDate);
public sealed record UpdateIncomeRequest(string Description, decimal Amount, DateOnly IncomeDate);