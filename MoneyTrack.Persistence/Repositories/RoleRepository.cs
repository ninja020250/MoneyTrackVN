using Microsoft.EntityFrameworkCore;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Repositories;

public class RoleRepository(MoneyTrackDbContext _dbContext) : BaseRepository<RoleEntity>(_dbContext), IRoleRepository
{
    public Task<RoleEntity> GetByNameAsync(RoleName name)
    {
        return _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}