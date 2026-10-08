
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.ExpenseCategories;
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Commands.UpdateExpense;

public sealed class UpdateExpenseCommandHandler : ICommandHandler<UpdateExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpenseCategoryRepository _categoryRepository;

    public UpdateExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork,
    IExpenseCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense is null)
            return Result.Failure(ExpenseErrors.NotFound(request.ExpenseId));

        
        var descriptionResult = expense.UpdateDescription(request.Description);
        if (descriptionResult.IsFailure) return descriptionResult;

        var category = await _categoryRepository.GetByIdAsync(request.ExpenseCategoryId, cancellationToken);
        if (category is null)
            return Result.Failure(ExpenseErrors.InvalidExpenseCategory);

        var invalidSubCategory = request.SubCategoryIds.Except(category.SubCategories.Select(s => s.Id)).Any();
        if (invalidSubCategory)
            return Result.Failure(ExpenseErrors.SubCategoryDoesNotBelongToCategory);

        var categoryResult = expense.UpdateCategoryAndSubCategories(request.ExpenseCategoryId, request.SubCategoryIds);
        if (categoryResult.IsFailure) return categoryResult;

        var dateRangeResult = expense.UpdateDateRange(request.StartDate, request.FinishDate);
        if (dateRangeResult.IsFailure) return dateRangeResult;

        
        var amountResult = expense.UpdateTotalAmount(request.TotalAmount);
        if (amountResult.IsFailure) return amountResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}