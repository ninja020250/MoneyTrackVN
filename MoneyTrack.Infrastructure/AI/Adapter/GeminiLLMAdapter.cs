using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Infrastructure.AI.Adapter;

public class GeminiLLMAdapter : ILLMService
{
    private readonly IGeminiService _geminiService;

    public GeminiLLMAdapter(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    public async Task<AITransactionDto> ParseTransactionAsync(
        string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories)
    {
        return await _geminiService.ParseTransactionAsync(
            message, language, currencyUnit, categories);
    }

    public string BuildTransactionPrompt(
        string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories)
    {
        return _geminiService.BuildTransactionPrompt(
            message, language, currencyUnit, categories);
    }
}