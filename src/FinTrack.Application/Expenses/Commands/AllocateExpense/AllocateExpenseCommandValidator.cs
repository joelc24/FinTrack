using FluentValidation;

namespace FinTrack.Application.Expenses.Commands.AllocateExpense;

public sealed class AllocateExpenseCommandValidator : AbstractValidator<AllocateExpenseCommand>
{
    public AllocateExpenseCommandValidator()
    {
        RuleFor(command => command.ExpenseId)
            .NotEmpty()
            .WithMessage("El identificador del gasto debe ser un GUID válido.");

        RuleFor(command => command.ProfileId)
            .NotEmpty()
            .WithMessage("El identificador del perfil debe ser un GUID válido.");

        RuleFor(command => command.Amount)
            .GreaterThan(0)
            .WithMessage("El monto asignado no puede ser negativo o igual a cero.");
    }
}