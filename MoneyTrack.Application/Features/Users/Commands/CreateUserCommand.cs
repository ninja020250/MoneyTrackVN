using MediatR;

namespace MoneyTrack.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<CreateUserCommandResponse>
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}