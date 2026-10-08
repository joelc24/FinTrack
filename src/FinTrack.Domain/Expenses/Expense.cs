using FinTrack.Domain.Common;

namespace FinTrack.Domain.Expenses;

public sealed class Expense : BaseEntity
{

    private readonly List<ExpenseAllocation> _allocations = [];
    private readonly List<ExpenseSubCategoryTag> _subCategoryTags = [];

    public Guid Id { get; private set; }
    public Guid ExpenseCategoryId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Money TotalAmount { get; private set; } = Money.Zero;
    public Guid? AccountId { get; private set; }
    public DateRange DateRange { get; private set; } = default!;
    public Guid CreatedBy { get; private set; }

    public IReadOnlyCollection<ExpenseAllocation> Allocations => _allocations.AsReadOnly();
    public IReadOnlyList<ExpenseSubCategoryTag> SubCategoryTags => _subCategoryTags.AsReadOnly();

    private Expense() {}

   public static Result<Expense> Create(string description, decimal totalAmount, Guid expenseCategoryId, Guid createdBy,
    DateTime startDate, DateTime finishDate, IReadOnlyList<Guid> subCategoryIds, Guid? accountId = null)
    {
        if (Guid.Empty == expenseCategoryId)
            return Result.Failure<Expense>(ExpenseErrors.InvalidExpenseCategory);

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Expense>(ExpenseErrors.EmptyDescription);

        var totalAmountResult = Money.Create(totalAmount);
        if (totalAmountResult.IsFailure)
            return Result.Failure<Expense>(totalAmountResult.Error);

        var dateRangeResult = DateRange.Create(startDate, finishDate);
        if (dateRangeResult.IsFailure)
            return Result.Failure<Expense>(dateRangeResult.Error);
        
        var expense = new Expense
        {
            Id = Guid.CreateVersion7(),
            Description = description.Trim(),
            TotalAmount = totalAmountResult.Value,
            ExpenseCategoryId = expenseCategoryId,
            DateRange = dateRangeResult.Value,
            AccountId = accountId, 
            CreatedBy = createdBy
        };

        foreach (var subCategoryId in subCategoryIds.Distinct())
        {
            expense._subCategoryTags.Add(ExpenseSubCategoryTag.Create(expense.Id, subCategoryId));
        }   

        return Result.Success(expense);
    }

    public Result TagSubCategory(Guid subCategoryId)
    {
        if (_subCategoryTags.Any(t => t.ExpenseSubCategoryId == subCategoryId))
            return Result.Failure(ExpenseErrors.DuplicateSubCategoryTag);

        _subCategoryTags.Add(ExpenseSubCategoryTag.Create(Id, subCategoryId));
        return Result.Success();
    }
    
    public Result AllocateTo(Guid profileId, decimal amount, DateOnly? paidAt = null)
    {
        var amountResult = Money.Create(amount);
        if (amountResult.IsFailure)
            return amountResult;

        var alreadyAllocated = _allocations.Aggregate(Money.Zero, (acc, a) => a.AssignedAmount + acc);
        if (alreadyAllocated + amountResult.Value > TotalAmount)
            return Result.Failure(ExpenseErrors.OverAllocated);

        _allocations.Add(ExpenseAllocation.Create(profileId, Id, amountResult.Value, paidAt));
        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(ExpenseErrors.EmptyDescription);

        Description = description.Trim();
        return Result.Success();
    }

    public Result UpdateTotalAmount(decimal totalAmount)
    {
        var totalAmountResult = Money.Create(totalAmount);
        if (totalAmountResult.IsFailure)
            return totalAmountResult;
    
        var alreadyAllocated = _allocations.Aggregate(Money.Zero, (acc, a) => acc + a.AssignedAmount);
        if (alreadyAllocated > totalAmountResult.Value)
            return Result.Failure(ExpenseErrors.OverAllocated);
    
        TotalAmount = totalAmountResult.Value;
        return Result.Success();
    }

    public Result UpdateCategoryAndSubCategories(Guid expenseCategoryId, IReadOnlyList<Guid> subCategoryIds)
    {
        if (expenseCategoryId == Guid.Empty)
            return Result.Failure(ExpenseErrors.InvalidExpenseCategory);

        if (subCategoryIds is null || subCategoryIds.Count == 0)
            return Result.Failure(ExpenseErrors.AtLeastOneSubCategoryRequired);

        ExpenseCategoryId = expenseCategoryId;

        
        _subCategoryTags.Clear();
        foreach (var subCategoryId in subCategoryIds.Distinct())
            _subCategoryTags.Add(ExpenseSubCategoryTag.Create(Id, subCategoryId));

        return Result.Success();
    }
    public Result UpdateAccount(Guid? accountId)
    {
        if (accountId == Guid.Empty)
            return Result.Failure(ExpenseErrors.InvalidAccount);

        AccountId = accountId;
        return Result.Success();
    }

    public Result UpdateDateRange(DateTime startDate, DateTime finishDate)
    {
        var dateRangeResult = DateRange.Create(startDate, finishDate);
        if (dateRangeResult.IsFailure)
            return Result.Failure(dateRangeResult.Error!);

        DateRange = dateRangeResult.Value;
        return Result.Success();
    }

}