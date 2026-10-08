using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Incomes;

namespace FinTrack.Application.Incomes.Commands.UpdateIncome;

public sealed class UpdateIncomeCommandHandler : ICommandHandler<UpdateIncomeCommand>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    public UpdateIncomeCommandHandler(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _incomeRepository = incomeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _incomeRepository.GetByIdAsync(request.IncomeId, cancellationToken);
        if (income is null)
            return Result.Failure(IncomeErrors.NotFound(request.IncomeId));

        var descriptionResult = income.UpdateDescription(request.Description);
        if (descriptionResult.IsFailure) return descriptionResult;

        var today = DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);
        var dateResult = income.UpdateIncomeDate(request.IncomeDate, today);
        if (dateResult.IsFailure) return dateResult;

        var amountResult = Money.Create(request.Amount);
        if (amountResult.IsFailure) return amountResult;

        income.UpdateAmount(amountResult.Value); 

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}