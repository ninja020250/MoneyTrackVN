using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Api.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<TransactionResponse>> CreateTransaction(
        [FromBody] CreateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("bulk-create")]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<GetListTransactionResponse>> BulkCreateTransactions(
        [FromBody] BulkCreateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut("bulk-update")]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<GetListTransactionResponse>> BulkUpdateTransactions(
        [FromBody] BulkUpdateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPut]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<TransactionResponse>> UpdateTransaction(
        [FromBody] UpdateTransactionCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<GetListTransactionResponse>> GetTransactionsByUserId(
        [FromQuery] string userId)
    {
        var query = new GetTransactionsQuery() { UserId = userId };
        return await _mediator.Send(query);
    }
}