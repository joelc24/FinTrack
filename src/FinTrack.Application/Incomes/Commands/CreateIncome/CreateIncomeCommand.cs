
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Incomes.Dtos;

namespace FinTrack.Application.Incomes.Commands.CreateIncome;

public sealed record CreateIncomeCommand(string Description, decimal Amount, DateOnly IncomeDate) : ICommand<IncomeDto>;