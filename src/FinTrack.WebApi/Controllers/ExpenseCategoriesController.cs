
using FinTrack.Application.ExpenseCategories.Queries.GetAllExpenseCategories;
using FinTrack.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Controllers;

[Authorize(Policy = "SessionRequired")]
[Route("api/expense-categories")]
public sealed class ExpenseCategoriesController : ApiControllerBase
{
    private readonly ISender _sender;
    public ExpenseCategoriesController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _sender.Send(new GetAllExpenseCategoriesQuery(), ct);
        return HandleResult(result);
    }
}