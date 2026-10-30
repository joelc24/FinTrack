using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Domain.UnitTests.Expenses;

public class ExpenseTests
{

    //--------HELPERS----------------
    private static Result<Expense> CreateExpense(
        string description = "Mercado", 
        decimal totalAmount = 100_000m, 
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

    //------------------ CREATE ------------------
    [Fact]
    public void Create_WithValidData_ReturnSuccess()
    {
        var result = CreateExpense();

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public void Create_WithPaddedDescription_TrimsIt()
    {
        var result = CreateExpense(description: "   Mercado   ");

        Assert.True(result.IsSuccess);
        Assert.Equal("Mercado", result.Value.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    public void Create_WithBlankDescription_ReturnsEmptyDescriptionError(string invalidDescription)
    {
        var result = CreateExpense(description: invalidDescription);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.EmptyDescription, result.Error);
    }

    [Fact]
    public void Create_WithoutSubCategories_ReturnsAtLeastOneSubCategoryError()
    {
        var result = CreateExpense(subCategoryIds: []);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.AtLeastOneSubCategoryRequired, result.Error);
    }

    [Fact]
    public void Create_WithEmptyCreatedBy_ReturnsInvalidCreatedByError()
    {
        var result = CreateExpense(createdBy: Guid.Empty);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.InvalidCreatedBy, result.Error);
    }

    [Fact]
    public void Create_WithEmptyAccountId_ReturnsInvalidAccountError()
    {
        var result = CreateExpense(accountId: Guid.Empty);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.InvalidAccount, result.Error);
    }

    [Fact]
    public void Create_WithDuplicatedSubCategories_TagsEachOnlyOnce()
    {
        var subCategoryId = Guid.CreateVersion7();
        var result = CreateExpense(subCategoryIds: [subCategoryId, subCategoryId]);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.SubCategoryTags);
    }

    //-------------- ALLOCATETO ------------------
    [Fact]
    public void AllocateTo_WithAmountWithinTotal_AddsAllocation()
    {
        var expense = CreateExpense(totalAmount: 100_000m).Value;
        var result = expense.AllocateTo(Guid.CreateVersion7(), 40_000m);

        Assert.True(result.IsSuccess);
        Assert.Single(expense.Allocations);
    }

    [Fact]
    public void AllocateTo_WithAmountEqualToTotal_Succeeds()
    {
        var expense = CreateExpense(totalAmount: 100_000m).Value;
        var result = expense.AllocateTo(Guid.CreateVersion7(), 100_000m);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void AllocateTo_WhenSumExceedsTotal_ReturnsOverAllocatedError()
    {
        var expense = CreateExpense(totalAmount: 100_000m).Value;
        expense.AllocateTo(Guid.CreateVersion7(), 70_000m);
        
        var result = expense.AllocateTo(Guid.CreateVersion7(), 50_000m);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.OverAllocated, result.Error);
        Assert.Single(expense.Allocations); // Ensure the second allocation was not added
    }

    [Fact]
    public void AllocateTo_WhenDuplicateAllocation_ReturnsDuplicateAllocationError()
    {
        var profileId = Guid.CreateVersion7();
        var expense = CreateExpense(totalAmount: 100_000m).Value;
        expense.AllocateTo(profileId, 80_000m);

        var result = expense.AllocateTo(profileId, 20_000m);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.DuplicateAllocation, result.Error);
        Assert.Single(expense.Allocations); // Ensure the second allocation was not added
    }

    //--------------- UPDATETOTALAMOUNT ----------------
    [Fact]
    public void UpdateTotalAmount_BelowAllocatedSum_ReturnsOverAllocatedError()
    {
        var expense = CreateExpense(totalAmount: 100_000m).Value;
        expense.AllocateTo(Guid.CreateVersion7(), 80_000m);

        var result = expense.UpdateTotalAmount(70_000m);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.OverAllocated, result.Error);
        Assert.Equal(100_000m, expense.TotalAmount.Amount); // Ensure the total amount was not updated
    }

    [Fact]
    public void UpdateTotalAmount_EqualToAllocatedSum_Succeeds()
    {
        var expense = CreateExpense(totalAmount: 100_000m).Value;
        expense.AllocateTo(Guid.CreateVersion7(), 80_000m);

        var result = expense.UpdateTotalAmount(80_000m);

        Assert.True(result.IsSuccess);
    }

    //--------------- TAGSUBCATEGORY ----------------
    [Fact]
    public void TagSubCategory_WhenAlreadyTagged_ReturnsDuplicateError()
    {
        var subCategoryId = Guid.CreateVersion7();
        var expense = CreateExpense(subCategoryIds: [subCategoryId]).Value;

        var result = expense.TagSubCategory(subCategoryId);

        Assert.True(result.IsFailure);
        Assert.Equal(ExpenseErrors.DuplicateSubCategoryTag, result.Error);
        Assert.Single(expense.SubCategoryTags); // Ensure the subcategory was not added again
    }
}