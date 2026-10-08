
namespace FinTrack.Application.Common.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    Guid? AccountId { get; }
    Guid? ProfileId { get; }
    string? Email { get; }
    bool IsAdmin { get; }
}