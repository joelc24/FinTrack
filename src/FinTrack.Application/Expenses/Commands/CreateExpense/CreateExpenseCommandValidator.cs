using FluentValidation;

namespace FinTrack.Application.Expenses.Commands.CreateExpense;

public sealed class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("La descripción del gasto es obligatoria.");

        RuleFor(command => command.TotalAmount)
            .GreaterThan(0)
            .WithMessage("El monto del gasto no puede ser negativo o igual a cero.");

        RuleFor(command => command.ExpenseCategoryId)
            .NotEmpty()
            .WithMessage("El identificador de la categoría debe ser un GUID válido.");

        RuleFor(command => command.FinishDate)
            .GreaterThanOrEqualTo(command => command.StartDate)
            .WithMessage("La fecha final no puede ser anterior a la fecha inicial.");

        When(command => command.AccountId.HasValue, () =>
        {
            RuleFor(command => command.AccountId)
                .NotEmpty()
                .WithMessage("El identificador de la cuenta debe ser un GUID válido.");
        });
    }
}