using FluentValidation;

namespace FinTrack.Application.Incomes.Commands.CreateIncome;

public sealed class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator()
    {
        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("La descripción del ingreso es obligatoria.");

        RuleFor(command => command.Amount)
            .GreaterThan(0)
            .WithMessage("El monto del ingreso no puede ser negativo o igual a cero.");
    }
}