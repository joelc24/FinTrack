
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Incomes.Commands.UpdateIncome;

public sealed record UpdateIncomeCommand(Guid IncomeId, string Description, decimal Amount, DateOnly IncomeDate) : ICommand;