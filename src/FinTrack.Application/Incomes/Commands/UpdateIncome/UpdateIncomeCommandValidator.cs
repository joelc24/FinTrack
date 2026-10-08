using FluentValidation;

namespace FinTrack.Application.Incomes.Commands.UpdateIncome;

public sealed class UpdateIncomeCommandValidator : AbstractValidator<UpdateIncomeCommand>
{
    public UpdateIncomeCommandValidator()
    {
        RuleFor(command => command.IncomeId)
            .NotEmpty()
            .WithMessage("El identificador del ingreso debe ser un GUID válido.");

        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("La descripción del ingreso es obligatoria.");

        RuleFor(command => command.Amount)
            .GreaterThan(0)
            .WithMessage("El monto del ingreso no puede ser negativo o igual a cero.");
    }
}