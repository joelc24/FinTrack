
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Expenses.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Expenses;

namespace FinTrack.Application.Expenses.Queries.GetExpensesByProfileId;

public sealed class GetExpensesByProfileIdQueryHandler : IQueryHandler<GetExpensesByProfileIdQuery, IReadOnlyList<ExpenseDto>>
{
    private readonly IExpenseRepository _expenseRepository;
    public GetExpensesByProfileIdQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<IReadOnlyList<ExpenseDto>>> Handle(GetExpensesByProfileIdQuery request, CancellationToken cancellationToken)
    {
        var expenses =  await _expenseRepository.GetByProfileIdAsync(request.ProfileId, cancellationToken);
        
        var expenseDtos = expenses.Select(e => e.ToDto()).ToList();

        return Result.Success<IReadOnlyList<ExpenseDto>>(expenseDtos);
    }
}