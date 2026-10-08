namespace FinTrack.Domain.Incomes;

public interface IIncomeRepository
{
    Task<Income?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Income>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default);

    void Add(Income income);
    void Remove(Income income);
}