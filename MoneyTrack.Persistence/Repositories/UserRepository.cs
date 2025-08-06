using Microsoft.EntityFrameworkCore;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Repositories;

public class UserRepository(MoneyTrackDbContext _dbContext) : BaseRepository<UserEntity>(_dbContext), IUserRepository
{
    public async Task<UserEntity> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == email.ToLower() && u.DeletedDate == null);
    }
}