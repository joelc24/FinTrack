
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Expenses.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;
using FinTrack.Domain.ExpenseCategories;

namespace FinTrack.Application.Expenses.Commands.CreateExpense;

public sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IExpenseCategoryRepository _categoryRepository;

    public CreateExpenseCommandHandler(
        IExpenseRepository expenseRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser,
        IExpenseCategoryRepository categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ExpenseDto>> Handle(CreateExpenseCommand request, CancellationToken ct)
    {
        // Validación cruzada: existe la categoría Y cada subcategoría pertenece a ELLA.
        var category = await _categoryRepository.GetByIdAsync(request.ExpenseCategoryId, ct);
        if (category is null)
            return Result.Failure<ExpenseDto>(ExpenseErrors.InvalidExpenseCategory);

        var invalidSubCategory = request.SubCategoryIds
            .Except(category.SubCategories.Select(s => s.Id))
            .Any();
        if (invalidSubCategory)
            return Result.Failure<ExpenseDto>(ExpenseErrors.SubCategoryDoesNotBelongToCategory);

        var createResult = Expense.Create(
            request.Description, request.TotalAmount, request.ExpenseCategoryId,
            _currentUser.ProfileId!.Value, request.StartDate, request.FinishDate, request.SubCategoryIds, request.AccountId);

        if (createResult.IsFailure)
            return Result.Failure<ExpenseDto>(createResult.Error);

        var expense = createResult.Value;
        foreach (var subCategoryId in request.SubCategoryIds)
        {
            var tagResult = expense.TagSubCategory(subCategoryId);
            if (tagResult.IsFailure)
                return Result.Failure<ExpenseDto>(tagResult.Error);
        }

        _expenseRepository.Add(expense);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result.Success(expense.ToDto());
    }
}