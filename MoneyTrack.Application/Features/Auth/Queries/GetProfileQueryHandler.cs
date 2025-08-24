using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Models.User;

namespace MoneyTrack.Application.Features.Auth.Queries;

public class GetProfileQueryHandler(
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IMapper mapper
)
    : IRequestHandler<GetProfileQuery, UserDto>
{
    public async Task<UserDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
        {
            throw new UnauthenticatedException("Can not get profile");
        }

        var user = await userRepository.GetByIdWithRoleAsync(userId.Value);

        return mapper.Map<UserDto>(user);
    }
}