using MediatR;

namespace MoneyTrack.Application.Features.Users;

public class GetUserQuery : IRequest<GetUserResponse>
{
    public Guid UserId { get; set; }
}