namespace MoneyTrack.Application.Features.Users;

public class GetUserResponse
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime? DeletedDate { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public string? RefreshToken { get; set; } = string.Empty;

    public DateTime? RefreshTokenExpiryTime { get; set; }
}