using MediatR;
using MoneyTrack.Application.Models.Auth;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class VerifyOtpCommand : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Otp { get; set; }
}