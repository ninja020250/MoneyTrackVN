using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Users;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator _mediator) : ControllerBase
{
    [HttpGet("{userId}")]
    [Authorize(Roles = $"{nameof(RoleName.Guest)}, {nameof(RoleName.Admin)}")]
    public async Task<ActionResult<GetUserResponse>> GetUsers(string userId)
    {
        var userQuery = new GetUserQuery() { UserId = new Guid(userId) };
        return await _mediator.Send(userQuery);
    }
}