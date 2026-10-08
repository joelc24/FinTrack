
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Queries.GetExpenseById;

public sealed class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, ExpenseDto>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository) =>
        _expenseRepository = expenseRepository;

    public async Task<Result<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);
        return expense is null
            ? Result.Failure<ExpenseDto>(ExpenseErrors.NotFound(request.ExpenseId))
            : Result.Success(expense.ToDto());
    }
}