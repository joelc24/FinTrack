namespace FinTrack.Domain.Expenses;

public interface IExpenseRepository
{
    // Trae el aggregate COMPLETO (Expense + sus Allocations) — usar para comandos
    // que necesitan invariantes del aggregate completo (ej. AllocateTo, UpdateTotalAmount).
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Variantes de consulta específicas del dominio, no un GetAll genérico sin filtro.
    Task<IReadOnlyList<Expense>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Expense>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    void Add(Expense expense);
    void Remove(Expense expense);
}