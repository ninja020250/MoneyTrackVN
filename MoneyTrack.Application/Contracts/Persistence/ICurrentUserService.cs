namespace MoneyTrack.Application.Contracts.Persistence;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}