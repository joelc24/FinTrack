namespace FinTrack.Domain.ExpenseCategories;

using FinTrack.Domain.Common;

public class ExpenseCategoryErrors
{
	public static readonly Error EmptyTitle = Error.Validation(
		"ExpenseCategory.EmptyTitle", "El titulo es requerido.");

	public static readonly Error EmptyDescription = Error.Validation(
		"ExpenseCategory.EmptyDescription", "La descripcion es requerida.");

	public static readonly Error InvalidExpenseCategory = Error.Validation(
		"ExpenseCategory.InvalidExpenseCategory", "La categoria de gasto es invalida.");
	
	public static readonly Error DuplicateSubCategory = Error.Validation(
		"ExpenseCategory.DuplicateSubCategory", "La subcategoria de gasto es ya se encuentra registrada.");
}