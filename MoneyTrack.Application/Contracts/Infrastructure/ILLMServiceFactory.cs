namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface ILLMServiceFactory
{
    ILLMService CreateLLMService(string providerName);
    ILLMService CreateDefaultLLMService();
}