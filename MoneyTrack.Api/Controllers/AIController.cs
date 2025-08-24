using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Features.AI;
using MoneyTrack.Application.Models.AI;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Api.Controllers;

[Route("api/ai")]
[ApiController]
public class AIController(IMediator _mediator, ICurrentUserService _currentUserService) : ControllerBase
{
    [HttpPost("transform-transaction")]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<CreateTransactionFromMessageCommandResponse>> CreateTransaction(
        [FromBody] CreateTransactionFromMessageRequest request)
    {
        var userId = _currentUserService.UserId
                     ?? throw new UnauthenticatedException("User has no right!");
        var command = new CreateTransactionFromMessageCommand
        {
            Message = request.Message,
            UserId = userId
        };
        return await _mediator.Send(command);
    }

    [HttpPost("transform-transaction/free")]
    public async Task<ActionResult<CreateTransactionFromMessageCommandResponse>> CreateTransactionFree(
        [FromBody] CreateTransactionFromMessageRequest request)
    {
        var command = new CreateTransactionFromMessageCommand
        {
            Message = request.Message,
        };
        var response = await _mediator.Send(command);
        return response;
    }
}