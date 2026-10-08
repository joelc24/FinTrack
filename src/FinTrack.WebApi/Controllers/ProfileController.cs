
using FinTrack.Application.Profiles.Commands.ActiveProfileProtection;
using FinTrack.Application.Profiles.Commands.AddProfile;
using FinTrack.Application.Profiles.Commands.DeactiveProfileProtection;
using FinTrack.Application.Profiles.Commands.UpdateProfile;
using FinTrack.Application.Profiles.Queries.GetProfilesByAccount;
using FinTrack.WebApi.Common;
using FinTrack.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Controllers;

[Authorize]
[Route("api/profiles")]
public sealed class ProfilesController : ApiControllerBase
{
    private readonly ISender _sender;
    public ProfilesController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetMyAccountProfiles(CancellationToken ct)
    {
        var result = await _sender.Send(new GetProfilesByAccountQuery(), ct);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(AddProfileRequest request, CancellationToken ct)
    {
        var command = new AddProfileCommand(request.Name, request.LastName, request.UserName, request.ImageUrl);
        var result = await _sender.Send(command, ct);
        return HandleResult(result, id => CreatedAtAction(nameof(GetMyAccountProfiles), null, new { id }));
    }

    [Authorize(Policy = "SessionRequired")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProfileRequest request, CancellationToken ct)
    {
        var command = new UpdateProfileCommand(id, request.Name, request.LastName, request.ImageUrl);
        var result = await _sender.Send(command, ct);
        return HandleResult(result);
    }

    [Authorize(Policy = "SessionRequired")]
    [HttpPost("{id:guid}/protection/activate")]
    public async Task<IActionResult> ActivateProtection(Guid id, ActivateProfileProtectionRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new ActivateProfileProtectionCommand(id, request.Pin), ct);
        return HandleResult(result);
    }

    [Authorize(Policy = "SessionRequired")]
    [HttpPost("{id:guid}/protection/deactivate")]
    public async Task<IActionResult> DeactivateProtection(Guid id, DeactivateProfileProtectionRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new DeactivateProfileProtectionCommand(id, request.Pin), ct);
        return HandleResult(result);
    }
}