using FinTrack.Domain.Expenses;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public sealed class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _dbContext;

    public ExpenseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Expenses
            .Include(expense => expense.Allocations)
            .FirstOrDefaultAsync(expense => expense.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Expense>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default) =>
        await _dbContext.Expenses
            .AsNoTracking()
            .Include(expense => expense.Allocations)
            .Where(expense => expense.Allocations.Any(allocation => allocation.ProfileId == profileId))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Expense>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) =>
        await _dbContext.Expenses
            .AsNoTracking()
            .Include(expense => expense.Allocations)
            .Where(expense => expense.AccountId == accountId)
            .ToListAsync(cancellationToken);

    public void Add(Expense expense) => _dbContext.Expenses.Add(expense);

    public void Remove(Expense expense) => _dbContext.Expenses.Remove(expense);
}