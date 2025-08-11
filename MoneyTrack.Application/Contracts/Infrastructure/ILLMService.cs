using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface ILLMService
{
    Task<CreateTransactionRequest> ParseTransactionAsync(
        string message,
        string language,
        string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories,
        Guid userId);

    string BuildTransactionPrompt(
        string message,
        string language,
        string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories);
}