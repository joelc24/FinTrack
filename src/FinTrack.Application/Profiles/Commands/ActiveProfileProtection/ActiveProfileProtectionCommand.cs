
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Profiles.Commands.ActiveProfileProtection;

public sealed record ActivateProfileProtectionCommand(Guid ProfileId, string ProfilePassword) : ICommand;