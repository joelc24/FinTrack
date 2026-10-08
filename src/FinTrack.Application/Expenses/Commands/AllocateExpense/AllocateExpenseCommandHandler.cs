
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Commands.AllocateExpense;

public sealed class AllocateExpenseCommandHandler : ICommandHandler<AllocateExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AllocateExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AllocateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense is null)
            return Result.Failure(ExpenseErrors.NotFound(request.ExpenseId));

        var allocateResult = expense.AllocateTo(request.ProfileId, request.Amount, request.PaidAt);
        if (allocateResult.IsFailure)
            return allocateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}