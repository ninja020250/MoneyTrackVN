using System.Text.Json.Serialization;

namespace MoneyTrack.Domain.Entities;

public class UserRoleEntity
{
    public Guid UserId { get; set; }
    
    [JsonIgnore]
    public UserEntity User { get; set; }

    public Guid RoleId { get; set; }
    
    
    public RoleEntity Role { get; set; }
}