using MoneyTrack.Application.Models;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}