

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Incomes.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Incomes;

namespace FinTrack.Application.Incomes.Queries.GetIncomesByProfile;

public sealed class GetIncomesByProfileQueryHandler : IQueryHandler<GetIncomesByProfileQuery, IReadOnlyList<IncomeDto>>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    public GetIncomesByProfileQueryHandler(IIncomeRepository incomeRepository,
        ICurrentUserService currentUserService
    )
    {
        _currentUserService = currentUserService;
        _incomeRepository = incomeRepository;
    }

    public async Task<Result<IReadOnlyList<IncomeDto>>> Handle(GetIncomesByProfileQuery request, CancellationToken cancellationToken)
    {
        if (request.ProfileId != _currentUserService.ProfileId && !_currentUserService.IsAdmin)
            return Result.Failure<IReadOnlyList<IncomeDto>>(IncomeErrors.NotOwner);

        
        var incomes = await _incomeRepository.GetByProfileIdAsync(request.ProfileId, cancellationToken);
        var listIncomeDto = incomes.Select(i => i.ToDto()).ToList();

        return Result.Success<IReadOnlyList<IncomeDto>>(listIncomeDto);
    }
}