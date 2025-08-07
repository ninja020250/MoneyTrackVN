using AutoMapper;
using MoneyTrack.Application.Features.Users;
using MoneyTrack.Application.Features.Users.Commands;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, GetUserResponse>()
            .ForMember(dest => dest.Roles, opt
                => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
            .ReverseMap();
        CreateMap<UserEntity, CreateUserCommand>().ReverseMap();
        CreateMap<RoleEntity, RoleDTO>();
    }
}