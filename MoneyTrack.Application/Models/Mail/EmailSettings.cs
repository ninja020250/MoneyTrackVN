namespace MoneyTrack.Application.Models;

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public string SmtpPort { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}