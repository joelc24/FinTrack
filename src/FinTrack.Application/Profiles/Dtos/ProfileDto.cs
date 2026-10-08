

namespace FinTrack.Application.Profiles.Dtos;

public sealed record ProfileDto(Guid Id, string Name, string LastName, string? ImageUrl, bool IsAdmin, bool IsProtected);