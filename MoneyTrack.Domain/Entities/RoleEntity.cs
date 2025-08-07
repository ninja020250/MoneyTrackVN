using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyTrack.Domain.Entities;

public enum RoleName
{
    Admin,
    Guest
}

public class RoleEntity : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RoleName Name { get; set; }

    public string Description { get; set; } = String.Empty;

    [JsonIgnore]
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
}