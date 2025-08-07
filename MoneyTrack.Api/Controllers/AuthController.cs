using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Auth.Commands;
using MoneyTrack.Application.Models.Auth;

namespace MoneyTrack.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Username = request.Username,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };

        return await _mediator.Send(command);
    }

    [HttpGet("request-otp")]
    public async Task<ActionResult> Register([FromQuery] string email)
    {
        var command = new RequestOtpCommand() { Email = email };

        await _mediator.Send(command);
        return Ok();
    }
}