using System.Security.Claims;
using System.Text;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Accounts;
using FinTrack.Domain.Profiles;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FinTrack.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly TimeProvider _timeProvider;
    public JwtTokenGenerator(JwtSettings jwtSettings, TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _jwtSettings = jwtSettings;
        
    }

    public (string token, DateTime expiresAtUtc) GenerateAccountToken(Account account)
    {
        return GenerateToken(account);
    }

    public (string token, DateTime expiredAtUtc) GenerateSessionToken(Account account, Profile profile)
    {
        return GenerateToken(account, profile);
    }

    private (string token, DateTime expiresAtUtc) GenerateToken(Account account, Profile? profile = null)
    {
        var expiresAtUtc = _timeProvider.GetUtcNow().AddMinutes(_jwtSettings.ExpiredMinutes).DateTime;

        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = account.Id.ToString(),
            [JwtRegisteredClaimNames.Email] = account.Email,
            [JwtRegisteredClaimNames.Jti] = Guid.CreateVersion7()
        };

        if(profile is not null)
        {
            claims.Add(ClaimTypes.Role, profile.IsAdmin ? "Admin" : "User");
            claims.Add("profile_id", profile.Id.ToString());
        }
        
        claims["token_type"] = profile is null ? "account" : "session";
        
        var signinkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = expiresAtUtc,
            SigningCredentials = signingCredentials
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);
        return (token, expiresAtUtc);
    }
}