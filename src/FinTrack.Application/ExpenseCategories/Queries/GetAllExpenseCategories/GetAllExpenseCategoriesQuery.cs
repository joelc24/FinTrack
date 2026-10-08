
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.ExpenseCategories.Dtos;

namespace FinTrack.Application.ExpenseCategories.Queries.GetAllExpenseCategories;

public sealed record GetAllExpenseCategoriesQuery : IQuery<IReadOnlyList<ExpenseCategoryDto>>;
