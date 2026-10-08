using FinTrack.Application.Accounts.Commands.LoginAccount;
using FinTrack.Application.Accounts.Commands.RegisterAccount;
using FinTrack.Application.Accounts.Commands.SelectProfile;
using FinTrack.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Controllers;


[Route("api/[controller]")]
public sealed class AuthController : ApiControllerBase
{
    private readonly ISender _sender;
    public AuthController(ISender sender) => _sender = sender;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterAccountCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAccountCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return HandleResult(result);
    }

    [Authorize(Policy = "AccountOnly")]
    [HttpPost("select-profile")]
    public async Task<IActionResult> SelectProfile(SelectProfileCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return HandleResult(result);
    }
}