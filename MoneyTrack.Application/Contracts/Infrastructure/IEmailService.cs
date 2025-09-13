using MoneyTrack.Application.Models;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task SendEmailAsync(Email email);
    
    Task SendEmailByApiAsync(Email email);
}