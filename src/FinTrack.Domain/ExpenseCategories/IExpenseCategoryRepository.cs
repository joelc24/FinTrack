namespace FinTrack.Domain.ExpenseCategories;

public interface IExpenseCategoryRepository
{
    Task<ExpenseCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // GetAllAsync SÍ tiene sentido aquí: es un catálogo pequeño (para poblar un dropdown),
    // a diferencia de Expense, donde traer TODOS sin filtro sería peligroso a escala.
    Task<IReadOnlyList<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken = default);

    void Add(ExpenseCategory category);
    void Remove(ExpenseCategory category);
}