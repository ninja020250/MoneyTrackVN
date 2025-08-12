using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IGeminiService
{
    Task<AITransactionDto> ParseTransactionAsync(
        string message,
        string language,
        string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories);

    string BuildTransactionPrompt(
        string message,
        string language,
        string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories);
}