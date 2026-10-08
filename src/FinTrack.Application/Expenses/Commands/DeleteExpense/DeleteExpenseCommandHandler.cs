
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Commands.DeleteExpense;

public sealed class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        if (expense is null)
            return Result.Failure(ExpenseErrors.NotFound(request.ExpenseId));

        _expenseRepository.Remove(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}