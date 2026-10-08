using FinTrack.Application.Common.Messaging;
using FinTrack.Application.ExpenseCategories.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.ExpenseCategories;

namespace FinTrack.Application.ExpenseCategories.Queries.GetAllExpenseCategories;

public sealed class GetAllExpenseCategoriesQueryHandler : IQueryHandler<GetAllExpenseCategoriesQuery, IReadOnlyList<ExpenseCategoryDto>>
{
    private readonly IExpenseCategoryRepository _expenseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    public GetAllExpenseCategoriesQueryHandler(IExpenseCategoryRepository expenseCategoryRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _expenseCategoryRepository = expenseCategoryRepository;
    }

    public async Task<Result<IReadOnlyList<ExpenseCategoryDto>>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        var listExpenseCategory = await _expenseCategoryRepository.GetAllAsync(cancellationToken);
        var listExpenseCategoryDto = listExpenseCategory.Select(ec => ec.ToDto()).ToList();

        return Result.Success<IReadOnlyList<ExpenseCategoryDto>>(listExpenseCategoryDto);
    }
} 