namespace FinTrack.Domain.ExpenseCategories;

public class ExpenseSubCategory
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid ExpenseCategoryId { get; private set; }
    
    private ExpenseSubCategory() {}

    internal static ExpenseSubCategory Create(Guid expenseCategoryId, string title, string description)
    {
        var subCategory = new ExpenseSubCategory
        {
            Id = Guid.CreateVersion7(),
            Title = title.Trim(),
            Description = description.Trim(),
            ExpenseCategoryId = expenseCategoryId
        };
        return subCategory;
    }

}