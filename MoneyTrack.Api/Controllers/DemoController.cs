using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Users;

namespace MoneyTrack.Api.Controllers;

[Route("api/demo")]
[ApiController]
public class DemoController(IMediator _mediator) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<string>> Demo()
    {
        var userQuery = new GetUserQuery() { UserId = new Guid("d3f0a8f4-95e9-4fc5-8d87-4b7bfa470fc9") };
        var res =  await _mediator.Send(userQuery);
        if (res == null)
        {
            return NotFound("User not found");
        }
        return Ok(res.Email);
    }
}