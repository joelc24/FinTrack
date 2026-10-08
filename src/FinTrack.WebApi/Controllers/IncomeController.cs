
using FinTrack.Application.Incomes.Commands.CreateIncome;
using FinTrack.Application.Incomes.Commands.DeleteIncome;
using FinTrack.Application.Incomes.Commands.UpdateIncome;
using FinTrack.Application.Incomes.Queries.GetIncomeById;
using FinTrack.Application.Incomes.Queries.GetIncomesByProfile;
using FinTrack.WebApi.Common;
using FinTrack.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Controllers;

[Authorize(Policy = "SessionRequired")]
[Route("api/incomes")]
public sealed class IncomesController : ApiControllerBase
{
    private readonly ISender _sender;
    public IncomesController(ISender sender) => _sender = sender;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetIncomeByIdQuery(id), ct);
        return HandleResult(result);
    }

    [HttpGet("profile/{profileId:guid}")]
    public async Task<IActionResult> GetByProfile(Guid profileId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetIncomesByProfileQuery(profileId), ct);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateIncomeRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateIncomeCommand(request.Description, request.Amount, request.IncomeDate), ct);
        return HandleResult(result, income => CreatedAtAction(nameof(GetById), new { id = income.Id }, income));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateIncomeRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new UpdateIncomeCommand(id, request.Description, request.Amount, request.IncomeDate), ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new DeleteIncomeCommand(id), ct);
        return HandleResult(result);
    }
}