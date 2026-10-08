using FinTrack.Domain.ExpenseCategories;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public sealed class ExpenseCategoryRepository : IExpenseCategoryRepository
{
    private readonly AppDbContext _dbContext;

    public ExpenseCategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExpenseCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.ExpenseCategories
            .Include(category => category.SubCategories)
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.ExpenseCategories
            .AsNoTracking()
            .Include(category => category.SubCategories)
            .OrderBy(category => category.Title)
            .ToListAsync(cancellationToken);

    public void Add(ExpenseCategory category) => _dbContext.ExpenseCategories.Add(category);

    public void Remove(ExpenseCategory category) => _dbContext.ExpenseCategories.Remove(category);
}