using MoneyTrack.Application.Features.Users;

namespace MoneyTrack.Application.Models.User;

public class UserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime? DeletedAt { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public string? RefreshToken { get; set; } = string.Empty;

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ICollection<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
}