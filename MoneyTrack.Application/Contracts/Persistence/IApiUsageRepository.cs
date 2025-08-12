using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IApiUsageRepository : IAsyncRepository<ApiUsageEntity>
{
    Task<ApiUsageEntity> getTodayUsageByName(Guid userId, string name);
}