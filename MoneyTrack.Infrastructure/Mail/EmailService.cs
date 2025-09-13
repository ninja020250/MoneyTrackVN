using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models;

namespace MoneyTrack.Infrastructure.Mail;
public class EmailService : IEmailService
{
    private readonly string _SenderName;
    private readonly string _SenderEmail;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public EmailService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        var emailSettings = _configuration.GetSection("EmailSettings");
        _SenderName = emailSettings["SenderName"];
        _SenderEmail = emailSettings["SenderEmail"];
    }

    public async Task SendEmailAsync(Email email)
    {
        // Redirect to API-based email sending
        await SendEmailByApiAsync(email);
    }
    
    public async Task SendEmailByApiAsync(Email email)
    {
        var apiKey = _configuration["EmailSettings:BrevoApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Brevo API key is not configured");
        }

        var requestBody = new
        {
            sender = new
            {
                name = _SenderName,
                email = _SenderEmail
            },
            to = new[]
            {
                new
                {
                    email = email.To,
                    name = email.To
                }
            },
            subject = email.Subject,
            htmlContent = email.Body
        };

        var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

        var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to send email via Brevo API. Status: {response.StatusCode}, Content: {errorContent}");
        }
    }
}