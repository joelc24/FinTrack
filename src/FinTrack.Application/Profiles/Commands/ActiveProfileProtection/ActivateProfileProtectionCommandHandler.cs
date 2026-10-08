

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Profiles.Commands.ActiveProfileProtection;

public sealed class ActivateProfileProtectionCommandHandler : ICommandHandler<ActivateProfileProtectionCommand>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ActivateProfileProtectionCommandHandler(
        IProfileRepository profileRepository, IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(ActivateProfileProtectionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Pin))
            return Result.Failure(ProfileErrors.PasswordInvalid);

        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        if (profile is null)
            return Result.Failure(ProfileErrors.NotFound(request.ProfileId));

        if (profile.AccountId != _currentUser.AccountId)
            return Result.Failure(ProfileErrors.NotOwnedByAccount);

        
        if (profile.Id != _currentUser.ProfileId)
            return Result.Failure(ProfileErrors.NotOwner);

        
        var pinHash = _passwordHasher.Hash(request.Pin);

        var activateResult = profile.ActivateProtection(pinHash);
        if (activateResult.IsFailure)
            return activateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}