using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Domain.UnitTests.Expenses;

public class ExpenseTests
{

    //--------HELPERS----------------
    private static Result<Expense> CreateExpense(
        string description, 
        decimal totalAmount, 
        Guid? expenseCategoryId = null, 
        Guid? createdBy = null, 
        IReadOnlyList<Guid>? subCategoryIds = null,
        Guid? accountId = null
    ) => Expense.Create(
        description,
        totalAmount,
        expenseCategoryId ?? Guid.CreateVersion7(),
        createdBy ?? Guid.CreateVersion7(),
        new DateTime(2026, 10, 1),
        new DateTime(2026, 10, 31),
        subCategoryIds ?? [Guid.CreateVersion7()],
        accountId
    );

    
}