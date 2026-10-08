

using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Accounts.Commands.SelectProfile;

public sealed record SelectProfileCommand(Guid ProfileId, string? Pin) : ICommand<SessionResponseDto>;

public sealed record SessionResponseDto(
    Guid AccountId, Guid ProfileId, string ProfileName, bool IsAdmin, string Token, DateTime ExpiresAtUtc);