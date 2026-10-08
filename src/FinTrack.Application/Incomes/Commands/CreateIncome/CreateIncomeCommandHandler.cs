
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Incomes.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Incomes;

namespace FinTrack.Application.Incomes.Commands.CreateIncome;

public sealed class CreateIncomeCommandHandler : ICommandHandler<CreateIncomeCommand, IncomeDto>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly TimeProvider _timeProvider;
    public CreateIncomeCommandHandler(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, TimeProvider timeProvider)
    {
        _incomeRepository = incomeRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _timeProvider = timeProvider;
    }

    public async Task<Result<IncomeDto>> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var amountResult = Money.Create(request.Amount);
        if (amountResult.IsFailure)
            return Result.Failure<IncomeDto>(amountResult.Error);

        var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);    

        var createResult = Income.Create(request.Description, amountResult.Value, request.IncomeDate, _currentUser.ProfileId!.Value, today);
        if (createResult.IsFailure)
            return Result.Failure<IncomeDto>(createResult.Error);

        _incomeRepository.Add(createResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(createResult.Value.ToDto());
    }
}