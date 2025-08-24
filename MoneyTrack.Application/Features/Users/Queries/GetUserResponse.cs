using MoneyTrack.Application.Models.User;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Users;

public class RoleDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } = String.Empty;
}

public class GetUserResponse : UserDto
{
}