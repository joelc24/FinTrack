
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Accounts;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Accounts.Commands.RegisterAccount;

public sealed class RegisterAccountCommandHandler : ICommandHandler<RegisterAccountCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAccountCommandHandler(
        IAccountRepository accountRepository, IProfileRepository profileRepository,
        IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var existing = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
            return Result.Failure<Guid>(AccountErrors.EmailAlreadyExists);

        var accountResult = Account.Create(request.Email, _passwordHasher.Hash(request.Password));
        if (accountResult.IsFailure)
            return Result.Failure<Guid>(accountResult.Error);

 
        var profileResult = Profile.Create(
            request.Name, request.LastName, request.UserName, accountResult.Value.Id, isAdmin: true);

        if (profileResult.IsFailure)
            return Result.Failure<Guid>(profileResult.Error);

        _accountRepository.Add(accountResult.Value);
        _profileRepository.Add(profileResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(accountResult.Value.Id);
    }
}