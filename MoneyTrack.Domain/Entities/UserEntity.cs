using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyTrack.Domain.Entities;

public class UserEntity : AuditableEntity
{
    [Key] public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [JsonIgnore] public string PasswordHash { get; set; } = String.Empty;

    public string? RefreshToken { get; set; } = string.Empty;

    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    
    [JsonIgnore] public ICollection<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
}