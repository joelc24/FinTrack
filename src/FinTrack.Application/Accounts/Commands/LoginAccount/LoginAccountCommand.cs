

using FinTrack.Application.Accounts.Commands.Dtos;
using FinTrack.Application.Common.Messaging;

namespace FinTrack.Application.Accounts.Commands.LoginAccount;

public sealed record LoginAccountCommand(string Email, string Password) : ICommand<AccountLoginResponseDto>;