using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTrack.Application.Contracts.Infrastructure;

namespace MoneyTrack.Infrastructure.AI.Adapter;

public class LLMServiceFactory : ILLMServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public LLMServiceFactory(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public ILLMService CreateLLMService(string providerName)
    {
        return providerName.ToLower() switch
        {
            "gemini" => _serviceProvider.GetRequiredService<GeminiLLMAdapter>(),
            "openai" => _serviceProvider.GetRequiredService<OpenAILLMAdapter>(),
            _ => throw new NotSupportedException($"Provider {providerName} not supported")
        };
    }

    public ILLMService CreateDefaultLLMService()
    {
        var defaultProvider = _configuration["LLMSettings:DefaultProvider"] ?? "gemini";
        return CreateLLMService(defaultProvider);
    }
}