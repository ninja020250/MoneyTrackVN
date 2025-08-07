using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models;

namespace MoneyTrack.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly string _SenderName;
    private readonly string _SenderEmail;
    private readonly string _SmtpServer;
    private readonly string _SmtpPort;
    private readonly string _Username;
    private readonly string _Password;
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        IConfiguration _configuration = configuration;
        var emailSettings = _configuration.GetSection("EmailSettings");
        _SenderName = emailSettings["SenderName"];
        _SenderEmail = emailSettings["SenderEmail"];
        _SmtpServer = emailSettings["SmtpServer"];
        _SmtpPort = emailSettings["SmtpPort"];
        _Username = emailSettings["Username"];
        _Password = emailSettings["Password"];
    }

    public async Task SendEmailAsync(Email email)
    {
        var sentEmail = new MimeMessage();

        sentEmail.From.Add(new MailboxAddress(_SenderName, _SenderEmail));
        sentEmail.To.Add(new MailboxAddress(email.To, email.To));
        sentEmail.Subject = email.Subject;
        var messages = email.Body;

        var bodyBuilder = new BodyBuilder { HtmlBody = email.Body };
        sentEmail.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_SmtpServer, int.Parse(_SmtpPort), false);
        await smtp.AuthenticateAsync(_Username, _Password);
        await smtp.SendAsync(sentEmail);
        await smtp.DisconnectAsync(true);
    }
}