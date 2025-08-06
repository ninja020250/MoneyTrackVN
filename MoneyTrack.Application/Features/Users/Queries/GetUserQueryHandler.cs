using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Users;

public class GetUserQueryHandler(IMapper _mapper, IAsyncRepository<UserEntity> _userRepository)
    : IRequestHandler<GetUserQuery, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new NotFoundException("User Id", request.UserId.ToString());
        }

        return _mapper.Map<GetUserResponse>(user);
    }
}