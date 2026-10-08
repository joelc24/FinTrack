

namespace FinTrack.Application.Accounts.Commands.Dtos;

public sealed record AccountLoginResponseDto(
    Guid AccountId, string AccountToken, DateTime ExpiresAtUtc, IReadOnlyList<ProfileSummaryDto> Profiles);

public sealed record ProfileSummaryDto(Guid Id, string Name, string? ImageUrl, bool IsProtected);