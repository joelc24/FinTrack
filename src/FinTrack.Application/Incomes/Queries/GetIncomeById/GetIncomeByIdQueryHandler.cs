

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Incomes.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Incomes;

namespace FinTrack.Application.Incomes.Queries.GetIncomeById;

public sealed class GetIncomeByIdQueryHandler : IQueryHandler<GetIncomeByIdQuery, IncomeDto>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    public GetIncomeByIdQueryHandler(IIncomeRepository incomeRepository, ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
        _incomeRepository = incomeRepository;
    }

    public async Task<Result<IncomeDto>> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
    {
        var income = await _incomeRepository.GetByIdAsync(request.IncomeId, cancellationToken);
        if(income is null)
            return Result.Failure<IncomeDto>(IncomeErrors.NotFound(request.IncomeId));

        if(income.ProfileId != _currentUserService.ProfileId && !_currentUserService.IsAdmin)
            return Result.Failure<IncomeDto>(IncomeErrors.NotOwner); 

        return Result.Success(income.ToDto());
    }
}