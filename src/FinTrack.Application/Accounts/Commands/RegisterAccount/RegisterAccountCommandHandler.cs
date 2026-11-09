
using FinTrack.Application.Accounts.Commands.Dtos;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Messaging;
using FinTrack.Domain.Accounts;
using FinTrack.Domain.Common;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Accounts.Commands.RegisterAccount;

public sealed class RegisterAccountCommandHandler : ICommandHandler<RegisterAccountCommand, AccountLoginResponseDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAccountCommandHandler(
        IAccountRepository accountRepository, IProfileRepository profileRepository,
        IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountLoginResponseDto>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var existing = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
            return Result.Failure<AccountLoginResponseDto>(AccountErrors.EmailAlreadyExists);

        var accountResult = Account.Create(request.Email, _passwordHasher.Hash(request.Password));
        if (accountResult.IsFailure)
            return Result.Failure<AccountLoginResponseDto>(accountResult.Error);

 
        var profileResult = Profile.Create(
            request.Name, request.LastName, request.UserName, accountResult.Value.Id, isAdmin: true);

        if (profileResult.IsFailure)
            return Result.Failure<AccountLoginResponseDto>(profileResult.Error);

        _accountRepository.Add(accountResult.Value);
        _profileRepository.Add(profileResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        (string token, DateTime expiresAtUtc) = _jwtTokenGenerator.GenerateAccountToken(accountResult.Value);
        
        var profile = profileResult.Value;
        var profileSummary = new ProfileSummaryDto(profile.Id, profile.Name, profile.ImageUrl, profile.IsProtected);
       
        return Result.Success(new AccountLoginResponseDto(
            accountResult.Value.Id, 
            token, 
            expiresAtUtc, 
            [profileSummary]
        ));
    }
}