

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Profiles.Commands.DeactiveProfileProtection;

public sealed class DeactivateProfileProtectionCommandHandler : ICommandHandler<DeactivateProfileProtectionCommand>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeactivateProfileProtectionCommandHandler(
        IProfileRepository profileRepository, IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(DeactivateProfileProtectionCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        if (profile is null)
            return Result.Failure(ProfileErrors.NotFound(request.ProfileId));

        if (profile.AccountId != _currentUser.AccountId)
            return Result.Failure(ProfileErrors.NotOwnedByAccount);

        var isSelf = profile.Id == _currentUser.ProfileId;
        if (!isSelf && !_currentUser.IsAdmin)
            return Result.Failure(ProfileErrors.NotOwner);

        // Chequeo de dominio ANTES de intentar verificar la CONTRASEÑA — si no está protegido,
        // ni siquiera llega a tocar PasswordHash (que sería null).
        if (!profile.IsProtected)
            return Result.Failure(ProfileErrors.ProfileUnprotected);

        // Solo exige la contraseña si eres TÚ mismo (no Admin actuando por otro) — la excepción
        // de "olvidé mi contraseña, ayúdame".
        if (isSelf && !_currentUser.IsAdmin)
        {
            if (string.IsNullOrWhiteSpace(request.ProfilePassword) || !_passwordHasher.Verify(request.ProfilePassword, profile.PasswordHash!))
                return Result.Failure(ProfileErrors.InvalidProfilePassword);
        }

        var deactivateResult = profile.DeactivateProtection();
        if (deactivateResult.IsFailure)
            return deactivateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}