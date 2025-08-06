using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Persistence;

public interface IUserRepository : IAsyncRepository<UserEntity>
{
    Task<UserEntity> GetByEmailAsync(string email);
}