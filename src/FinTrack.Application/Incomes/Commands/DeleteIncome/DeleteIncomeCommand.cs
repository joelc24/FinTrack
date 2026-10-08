
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Incomes.Commands.DeleteIncome;

public sealed record DeleteIncomeCommand(Guid IncomeId) : ICommand;

// Handler: idéntico ptrón a DeleteExpenseCommandHandler — GetByIdAsync, Remove, SaveChangesAsynca