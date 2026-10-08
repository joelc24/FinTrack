
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Dtos;

public static class ExpenseMappingExtensions
{
   public static ExpenseDto ToDto(this Expense expense) =>
    new(
        expense.Id,
        expense.Description,
        expense.TotalAmount.Amount,
        expense.ExpenseCategoryId,
        expense.AccountId,
        expense.CreatedBy,
        expense.DateRange.StartDate,
        expense.DateRange.FinishDate,
        expense.Allocations.Select(a => a.ToDto()).ToList(),
        expense.SubCategoryTags.Select(t => t.ExpenseSubCategoryId).ToList());
    public static ExpenseAllocationDto ToDto(this ExpenseAllocation allocation) =>
        new(
            allocation.Id, 
            allocation.ProfileId, 
            allocation.AssignedAmount.Amount, 
            allocation.PaidAt
        );
}