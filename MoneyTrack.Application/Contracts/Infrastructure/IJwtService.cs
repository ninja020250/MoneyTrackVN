using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface IJwtService
{
    string GenerateToken(UserEntity user);

    string GenerateRefreshToken();

    bool ValidateRefreshToken(string refreshToken, UserEntity user);

    string GetEmailFromToken(string token);
}