

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Incomes;

namespace FinTrack.Application.Incomes.Commands.DeleteIncome;

public sealed class DeleteIncomeCommandHandler : ICommandHandler<DeleteIncomeCommand>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteIncomeCommandHandler(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _incomeRepository = incomeRepository;
    }

    public async Task<Result> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _incomeRepository.GetByIdAsync(request.IncomeId, cancellationToken);
        if(income is null)
            return Result.Failure(IncomeErrors.NotFound(request.IncomeId));

        _incomeRepository.Remove(income);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}