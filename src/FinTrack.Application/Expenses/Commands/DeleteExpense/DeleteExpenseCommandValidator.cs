using FluentValidation;

namespace FinTrack.Application.Expenses.Commands.DeleteExpense;

public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(command => command.ExpenseId)
            .NotEmpty()
            .WithMessage("El identificador del gasto debe ser un GUID válido.");
    }
}