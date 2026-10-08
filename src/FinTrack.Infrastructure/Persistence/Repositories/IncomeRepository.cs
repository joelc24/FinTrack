using FinTrack.Domain.Incomes;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public sealed class IncomeRepository : IIncomeRepository
{
    private readonly AppDbContext _dbContext;

    public IncomeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Income?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Incomes
            .FirstOrDefaultAsync(income => income.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Income>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default) =>
        await _dbContext.Incomes
            .Where(income => income.ProfileId == profileId)
            .ToListAsync(cancellationToken);

    public void Add(Income income) => _dbContext.Incomes.Add(income);

    public void Remove(Income income) => _dbContext.Incomes.Remove(income);
}