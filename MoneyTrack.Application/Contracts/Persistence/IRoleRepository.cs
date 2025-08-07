using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Persistence;

public interface IRoleRepository : IAsyncRepository<RoleEntity>
{
    Task<RoleEntity> GetByNameAsync(RoleName name);
}