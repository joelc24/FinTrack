

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Profiles.Commands.AddProfile;

public sealed class AddProfileCommandHandler : ICommandHandler<AddProfileCommand, Guid>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AddProfileCommandHandler(IProfileRepository profileRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(AddProfileCommand request, CancellationToken cancellationToken)
    {
        
        var createResult = Profile.Create(
            request.Name, request.LastName, request.UserName,
            _currentUser.AccountId!.Value, isAdmin: false);

        if (createResult.IsFailure)
            return Result.Failure<Guid>(createResult.Error);

        _profileRepository.Add(createResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(createResult.Value.Id);
    }
}