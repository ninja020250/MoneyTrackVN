using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Api.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> CreateTransaction(
        [FromBody] CreateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<GetTransactionResponse>> BulkCreateTransactions(
        [FromBody] BulkCreateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPut]
    public async Task<ActionResult<TransactionResponse>> UpdateTransaction(
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