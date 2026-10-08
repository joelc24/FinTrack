using System.Security.Claims;
using FinTrack.Application.Common.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinTrack.WebApi.Common;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid? AccountId
    {
        get
        {
            var sub = User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public Guid? ProfileId
    {
        get
        {
            var profileId = User?.FindFirstValue("profile_id");
            return Guid.TryParse(profileId, out var id) ? id : null;
        }
    }

    public string? Email => User?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public bool IsAdmin => User?.IsInRole("Admin") ?? false;
}