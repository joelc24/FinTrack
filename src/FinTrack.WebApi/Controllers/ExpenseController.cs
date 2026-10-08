
using FinTrack.Application.Expenses.Commands.AllocateExpense;
using FinTrack.Application.Expenses.Commands.CreateExpense;
using FinTrack.Application.Expenses.Commands.DeleteExpense;
using FinTrack.Application.Expenses.Commands.UpdateExpense;
using FinTrack.Application.Expenses.Queries.GetExpenseById;
using FinTrack.Application.Expenses.Queries.GetExpensesByAccountId;
using FinTrack.Application.Expenses.Queries.GetExpensesByProfileId;
using FinTrack.WebApi.Common;
using FinTrack.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Controllers;

[Authorize(Policy = "SessionRequired")]
[Route("api/expenses")]
public sealed class ExpensesController : ApiControllerBase
{
    private readonly ISender _sender;
    public ExpensesController(ISender sender) => _sender = sender;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetExpenseByIdQuery(id), ct);
        return HandleResult(result);
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<IActionResult> GetByAccount(Guid accountId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetExpenseByAccountIdQuery(accountId), ct);
        return HandleResult(result);
    }

    [HttpGet("profile/{profileId:guid}")]
    public async Task<IActionResult> GetByProfile(Guid profileId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetExpensesByProfileIdQuery(profileId), ct);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseRequest request, CancellationToken ct)
    {
        var command = new CreateExpenseCommand(
            request.Description, request.TotalAmount, request.ExpenseCategoryId,
            request.StartDate, request.FinishDate, request.AccountId, request.SubCategoryIds);

        var result = await _sender.Send(command, ct);
        return HandleResult(result, expense => CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateExpenseRequest request, CancellationToken ct)
    {
        var command = new UpdateExpenseCommand(
            id, request.Description, request.TotalAmount, request.ExpenseCategoryId,
            request.StartDate, request.FinishDate, request.SubCategoryIds);
        var result = await _sender.Send(command, ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new DeleteExpenseCommand(id), ct);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/allocate")]
    public async Task<IActionResult> Allocate(Guid id, AllocateExpenseRequest request, CancellationToken ct)
    {
        var command = new AllocateExpenseCommand(id, request.ProfileId, request.Amount, request.PaidAt);
        var result = await _sender.Send(command, ct);
        return HandleResult(result);
    }
}