

using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Profiles.Commands.DeactiveProfileProtection;

public sealed record DeactivateProfileProtectionCommand(Guid ProfileId, string? Pin) : ICommand;