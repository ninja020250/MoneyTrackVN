using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Application.Features.Auth.Commands;
using MoneyTrack.Application.Features.Auth.Queries;
using MoneyTrack.Application.Models.Auth;
using MoneyTrack.Application.Models.User;

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
    public async Task<ActionResult> RequestOtp([FromQuery] string email)
    {
        var command = new RequestOtpCommand() { Email = email };

        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("verify-otp")]
    public async Task<ActionResult<LoginResponse>> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var command = new VerifyOtpCommand() { Email = request.Email, Otp = request.Otp };

        return await _mediator.Send(command);
    }

    [HttpPost("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        return await _mediator.Send(new GetProfileQuery());
    }
}