

using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Application.Profiles.Dtos;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Profiles.Queries.GetProfilesByAccount;

public sealed class GetProfilesByAccountQueryHandler : IQueryHandler<GetProfilesByAccountQuery, IReadOnlyList<ProfileDto>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUserService _currentUser;

    public GetProfilesByAccountQueryHandler(IProfileRepository profileRepository, ICurrentUserService currentUser)
    {
        _profileRepository = profileRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyList<ProfileDto>>> Handle(GetProfilesByAccountQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _profileRepository.GetByAccountIdAsync(_currentUser.AccountId!.Value, cancellationToken);

        IReadOnlyList<ProfileDto> dtos = profiles
            .Select(p => new ProfileDto(p.Id, p.Name, p.LastName, p.ImageUrl, p.IsAdmin, p.IsProtected))
            .ToList();

        return Result.Success(dtos);
    }
}