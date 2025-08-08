using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Persistence;

public interface ITransactionRepository : IAsyncRepository<TransactionEntity>
{
    public Task<List<TransactionEntity>> GetListByUserIdAsync(Guid userId);
    
    public Task<bool> IsTransactionExist(Guid id);
}