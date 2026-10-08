

using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Profiles.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(Guid ProfileId, string Name, string LastName, string? ImageUrl) : ICommand;