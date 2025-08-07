using MediatR;
using MoneyTrack.Application.Models.Auth;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class RequestOtpCommand : IRequest
{
    public string Email { get; set; }

    public string Template { get; set; }
}