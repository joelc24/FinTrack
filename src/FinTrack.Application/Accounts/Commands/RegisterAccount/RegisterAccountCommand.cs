

using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Accounts.Commands.RegisterAccount;

public sealed record RegisterAccountCommand(
    string Email, string Password, string Name, string LastName, string UserName) : ICommand<Guid>;