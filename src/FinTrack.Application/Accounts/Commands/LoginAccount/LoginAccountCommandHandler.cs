
using FinTrack.Application.Accounts.Commands.Dtos;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Accounts;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Accounts.Commands.LoginAccount;

public sealed class LoginCommandHandler : ICommandHandler<LoginAccountCommand, AccountLoginResponseDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IAccountRepository accountRepository, IProfileRepository profileRepository,
        IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _accountRepository = accountRepository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AccountLoginResponseDto>> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);

        
        if (account is null || !_passwordHasher.Verify(request.Password, account.PasswordHash))
            return Result.Failure<AccountLoginResponseDto>(AccountErrors.InvalidCredentials);

        var profiles = await _profileRepository.GetByAccountIdAsync(account.Id, cancellationToken);

        var (token, expiresAtUtc) = _jwtTokenGenerator.GenerateAccountToken(account);

        var profileSummaries = profiles
            .Select(p => new ProfileSummaryDto(p.Id, p.Name, p.ImageUrl, p.IsProtected))
            .ToList();

        return Result.Success(new AccountLoginResponseDto(account.Id, token, expiresAtUtc, profileSummaries));
    }
}