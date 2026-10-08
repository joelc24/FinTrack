

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Profiles.Commands.UpdateProfile;

public sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateProfileCommandHandler(IProfileRepository profileRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        if (profile is null)
            return Result.Failure(ProfileErrors.NotFound(request.ProfileId));

        // Regla de negocio de 2 partes
        // 1) el perfil debe pertenecer a MI cuenta 
        if (profile.AccountId != _currentUser.AccountId)
            return Result.Failure(ProfileErrors.NotOwnedByAccount);

        // 2) dentro de mi cuenta: soy YO ese perfil, o soy el Admin de la cuenta
        var isSelf = profile.Id == _currentUser.ProfileId;
        if (!isSelf && !_currentUser.IsAdmin)
            return Result.Failure(ProfileErrors.NotOwner);

        var nameResult = profile.UpdateName(request.Name);
        if (nameResult.IsFailure) return nameResult;

        var lastNameResult = profile.UpdateLastName(request.LastName);
        if (lastNameResult.IsFailure) return lastNameResult;

        profile.UpdateImageUrl(request.ImageUrl);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}