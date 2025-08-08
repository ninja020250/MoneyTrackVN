using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Features.Transactions.Queries;

namespace MoneyTrack.Api.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateTransactionCommandResponse>> CreateTransaction(
        [FromBody] CreateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPut]
    public async Task<ActionResult<CreateTransactionCommandResponse>> UpdateTransaction(
        [FromBody] UpdateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet]
    public async Task<ActionResult<GetTransactionResponse>> GetTransactionByUserId(
        [FromQuery] string userId)
    {
        var query = new GetTransactionsQuery() { UserId = userId };
        return await _mediator.Send(query);
    }
}