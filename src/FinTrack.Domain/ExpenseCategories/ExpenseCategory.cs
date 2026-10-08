using FinTrack.Domain.Common;

namespace FinTrack.Domain.ExpenseCategories;

public sealed class ExpenseCategory : BaseEntity
{
    private readonly List<ExpenseSubCategory> _subCategories = [];
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public IReadOnlyList<ExpenseSubCategory> SubCategories => _subCategories.AsReadOnly();

    private ExpenseCategory() {}


    public static Result<ExpenseCategory> Create(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<ExpenseCategory>(ExpenseCategoryErrors.EmptyTitle);

        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<ExpenseCategory>(ExpenseCategoryErrors.EmptyDescription);

        var expenseCategory = new ExpenseCategory
        {
            Id = Guid.CreateVersion7(),
            Title = title.Trim(),
            Description = description.Trim()
        };

        return Result.Success(expenseCategory);

    }

    public Result UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure(ExpenseCategoryErrors.EmptyTitle);

        Title = title;
        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(ExpenseCategoryErrors.EmptyDescription);

        Description = description;
        return Result.Success();
    }

    public Result AddSubCategory(string title, string description)
    {
        if (_subCategories.Any(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            return Result.Failure(ExpenseCategoryErrors.DuplicateSubCategory);

        _subCategories.Add(ExpenseSubCategory.Create(Id, title, description));
        return Result.Success();
    }
}