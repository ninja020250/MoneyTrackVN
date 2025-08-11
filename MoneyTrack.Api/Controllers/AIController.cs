using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.AI;
using MoneyTrack.Application.Features.Auth.Commands;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.AI;

namespace MoneyTrack.Api.Controllers;

[Route("api/ai")]
[ApiController]
public class AIController(IMediator _mediator) : ControllerBase
{
    private Guid GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }
    
    [HttpPost("create-transaction")]
    public async Task<ActionResult<CreateTransactionFromMessageCommandResponse>> Login([FromBody] CreateTransactionFromMessageRequest request)
    {
        // var userId = GetUserIdFromClaims(); // Extract from JWT claims TODO
    
        var command = new CreateTransactionFromMessageCommand
        {
            Message = request.Message,
            UserId = new Guid("e00a8847-1a6c-4932-8538-0520f79e70b0")
        };
    
        return await _mediator.Send(command);
    }
    
}