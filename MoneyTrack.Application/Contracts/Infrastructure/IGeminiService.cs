using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IGeminiService : ILLMService
{
    Task<CreateTransactionRequest> ParseMessageToObjectAsync(string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories, Guid userId);

    string BuildTransactionPrompt(string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories);
}