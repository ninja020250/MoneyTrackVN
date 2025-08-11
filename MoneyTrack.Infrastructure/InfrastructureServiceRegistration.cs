using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models;
using MoneyTrack.Infrastructure.AI;
using MoneyTrack.Infrastructure.Auth;
using MoneyTrack.Infrastructure.Mail;

namespace MoneyTrack.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<GeminiSettings>(configuration.GetSection("GeminiSettings"));
        
        services.AddHttpClient<IGeminiService, GeminiService>(client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "MoneyTrack/1.0");
        });
        
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        
        services.AddScoped<IGeminiService, GeminiService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IJwtService, JwtService>();
        
        services.AddSingleton<IOtpService, OtpService>();

        return services;
    }
}