

using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Profiles.Commands.AddProfile;

public sealed record AddProfileCommand(string Name, string LastName, string UserName, string? ImageUrl) : ICommand<Guid>;