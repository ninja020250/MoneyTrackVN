using MediatR;
using MoneyTrack.Application.Models.Auth;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<LoginResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}