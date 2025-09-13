using MediatR;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models;
using MoneyTrack.Application.Models.Auth;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class RequestOtpCommandHandler(IOtpService _otpService, IEmailService _emailService)
    : IRequestHandler<RequestOtpCommand>
{
    public async Task Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        var lowercaseEmail = request.Email.ToLower();
        var generatedOtp = _otpService.GenerateOtp(lowercaseEmail);

        // Read from embedded resource
        var assembly = typeof(RequestOtpCommandHandler).Assembly;
        var resourceName = "MoneyTrack.Application.Templates.otp_template.html";
    
        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        var templateContent = await reader.ReadToEndAsync();
        
        var message = templateContent.Replace("{otp}", generatedOtp).Replace("{name}", request.Email);

        var mail = new Email()
        {
            Body = message,
            Subject = "MoneyTrack OTP Code",
            To = lowercaseEmail,
        };

        await _emailService.SendEmailByApiAsync(mail);
    }
}