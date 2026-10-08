
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Queries.GetExpensesByAccountId;

public sealed class GetExpenseByAccountIdQueryHandler : IQueryHandler<GetExpenseByAccountIdQuery, IReadOnlyList<ExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICurrentUserService _currentUser;

    public GetExpenseByAccountIdQueryHandler(IExpenseRepository expenseRepository, ICurrentUserService currentUser)
    {
        _expenseRepository = expenseRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyList<ExpenseDto>>> Handle(GetExpenseByAccountIdQuery request, CancellationToken cancellationToken)
    {
        if (request.AccountId != _currentUser.AccountId)
            return Result.Failure<IReadOnlyList<ExpenseDto>>(ExpenseErrors.NotOwnedByAccount);

        var expenses = await _expenseRepository.GetByAccountIdAsync(request.AccountId, cancellationToken);
        return Result.Success<IReadOnlyList<ExpenseDto>>(expenses.Select(e => e.ToDto()).ToList());
    }
}