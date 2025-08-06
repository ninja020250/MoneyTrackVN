using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models;

namespace MoneyTrack.Infrastructure.Mail;

public class EmailService : IEmailService
{
    public Task<bool> SendEmail(Email email)
    {
        Console.WriteLine("Sending email");

        return Task.FromResult(true);
    }
}