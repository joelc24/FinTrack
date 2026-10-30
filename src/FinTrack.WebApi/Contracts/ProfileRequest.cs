
namespace FinTrack.WebApi.Contracts;

public sealed record AddProfileRequest(string Name, string LastName, string UserName, string? ImageUrl);
public sealed record UpdateProfileRequest(string Name, string LastName, string? ImageUrl);
public sealed record ActivateProfileProtectionRequest(string ProfilePassword);
public sealed record DeactivateProfileProtectionRequest(string? ProfilePassword);