using FluentValidation;

namespace FinTrack.Application.Incomes.Commands.DeleteIncome;

public sealed class DeleteIncomeCommandValidator : AbstractValidator<DeleteIncomeCommand>
{
    public DeleteIncomeCommandValidator()
    {
        RuleFor(command => command.IncomeId)
            .NotEmpty()
            .WithMessage("El identificador del ingreso debe ser un GUID válido.");
    }
}