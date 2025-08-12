using Microsoft.EntityFrameworkCore;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Repositories;

public class ApiUsageRepository(MoneyTrackDbContext _dbContext)
    : BaseRepository<ApiUsageEntity>(_dbContext), IApiUsageRepository
{
    public async Task<ApiUsageEntity> getTodayUsageByName(Guid userId, string name)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbContext.apiUsage
            .FirstOrDefaultAsync(apiUsage =>
                apiUsage.UserId == userId && apiUsage.CallDate == today && apiUsage.ApiName == name);
    }
}