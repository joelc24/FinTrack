using FinTrack.Domain.Common;

namespace FinTrack.Domain.Expenses;

public static class ExpenseErrors
{
    public static readonly Error EmptyDescription = Error.Validation("Expennse.EmptyDescription", "La descripcion es requerida");
    public static readonly Error InvalidExpenseCategory = Error.Validation("Expennse.InvalidExpenseCategory", "La categoria del gasto es invalida");
    public static readonly Error InvalidTotalAmount = Error.Validation("Expense.InvalidTotalAmount", "El total del gasto debe ser mayor que cero.");
    public static readonly Error InvalidAccount = Error.Validation("Expense.InvalidAccount", "La cuenta del gasto es invalida.");
    public static readonly Error NotOwnedByAccount = Error.Validation("Expense.NotOwnedByAccount", "No eres el propietario de la cuenta.");
    public static readonly Error InvalidCreatedBy = Error.Validation("Expense.InvalidCreatedBy", "El creador del gasto es invalido.");
    public static readonly Error OverAllocated = Error.Validation("Expense.OverAllocated", "La suma de asignaciones supera el total del gasto.");
    public static readonly Error DuplicateSubCategoryTag =
        Error.Conflict("Expense.DuplicateSubCategoryTag", "Esa subcategoría ya está asignada a este gasto.");

    public static readonly Error SubCategoryDoesNotBelongToCategory =
        Error.Validation("Expense.SubCategoryDoesNotBelongToCategory", "La subcategoría no pertenece a la categoría del gasto.");
    public static readonly Error AtLeastOneSubCategoryRequired =
        Error.Validation("Expense.AtLeastOneSubCategoryRequired", "Debe asignar al menos una subcategoría.");
    public static Error NotFound(Guid id) => Error.NotFound("Expense.NotFound", $"No se encontró el gasto con id '{id}'.");
}