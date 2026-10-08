

using FinTrack.Domain.Accounts;
using FinTrack.Domain.Profiles;

namespace FinTrack.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAtUtc) GenerateAccountToken(Account account);
    (string token, DateTime expiredAtUtc) GenerateSessionToken(Account account, Profile profile);
}