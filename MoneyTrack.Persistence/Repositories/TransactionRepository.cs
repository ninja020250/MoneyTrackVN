using Microsoft.EntityFrameworkCore;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Repositories;

public class TransactionRepository(MoneyTrackDbContext _dbContext)
    : BaseRepository<TransactionEntity>(_dbContext), ITransactionRepository
{
    public Task<List<TransactionEntity>> GetListByUserIdAsync(Guid userId)
    {
        return _dbContext.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task<bool> IsTransactionExist(Guid id)
    {
        var exists = await _dbContext.Transactions
            .AsNoTracking()
            .AnyAsync(c => c.Id == id);

        return exists;
    }
}