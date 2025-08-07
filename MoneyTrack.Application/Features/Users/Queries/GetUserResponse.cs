using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Users;

public class RoleDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } = String.Empty;
}

public class GetUserResponse
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