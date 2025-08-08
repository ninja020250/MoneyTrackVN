using Microsoft.EntityFrameworkCore;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Repositories;

public class TransactionCategoryRepository(MoneyTrackDbContext _dbContext)
    : BaseRepository<TransactionCategoryEntity>(_dbContext), ITransactionCategoryRepository
{
    public async Task<bool> IsCategoryCodeExist(string code)
    {
        var exists = await _dbContext.TransactionCategory
            .AsNoTracking()
            .AnyAsync(c => c.Code == code);

        return exists;
    }

    public Task<TransactionCategoryEntity?> GetByCodeAsync(string code)
    {
        var category = _dbContext.TransactionCategory.FirstOrDefaultAsync(c => c.Code == code);

        return category;
    }
}