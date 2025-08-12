using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Infrastructure.AI.Adapter;

public class OpenAILLMAdapter : ILLMService
{
    public Task<AITransactionDto> ParseTransactionAsync(string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories)
    {
        // TODO
        throw new NotImplementedException();
    }

    public string BuildTransactionPrompt(string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories)
    {
        // TODO
        throw new NotImplementedException();
    }
}