using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Persistence;

public interface ITransactionCategoryRepository : IAsyncRepository<TransactionCategoryEntity>
{
    public Task<bool> IsCategoryCodeExist(string code);

    public Task<TransactionCategoryEntity> GetByCodeAsync(string code);
}