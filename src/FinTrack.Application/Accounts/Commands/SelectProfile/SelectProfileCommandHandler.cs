
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Accounts;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Accounts.Commands.SelectProfile;

public sealed class SelectProfileCommandHandler : ICommandHandler<SelectProfileCommand, SessionResponseDto>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICurrentUserService _currentUser;

    public SelectProfileCommandHandler(
        IProfileRepository profileRepository, IAccountRepository accountRepository,
        IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, ICurrentUserService currentUser)
    {
        _profileRepository = profileRepository;
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _currentUser = currentUser;
    }

    public async Task<Result<SessionResponseDto>> Handle(SelectProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        if (profile is null)
            return Result.Failure<SessionResponseDto>(ProfileErrors.NotFound(request.ProfileId));

        // Verificación de seguridad CRÍTICA: el perfil elegido debe pertenecer
        // a la MISMA Account autenticada — si no, cualquier cuenta
        // podría "robar" el perfil de otra solo adivinando su Guid.
        if (profile.AccountId != _currentUser.AccountId)
            return Result.Failure<SessionResponseDto>(ProfileErrors.NotOwnedByAccount);

        if (profile.IsProtected)
        {
            if (string.IsNullOrWhiteSpace(request.Pin) || !_passwordHasher.Verify(request.Pin, profile.PasswordHash!))
                return Result.Failure<SessionResponseDto>(ProfileErrors.InvalidPin);
        }

        var account = await _accountRepository.GetByIdAsync(profile.AccountId, cancellationToken);
        var (token, expiresAtUtc) = _jwtTokenGenerator.GenerateSessionToken(account!, profile);

        return Result.Success(new SessionResponseDto(
            profile.AccountId, profile.Id, profile.Name, profile.IsAdmin, token, expiresAtUtc));
    }
}