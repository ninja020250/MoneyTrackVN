using AutoMapper;
using MediatR;
using MoneyTrack.Application.Models.User;

namespace MoneyTrack.Application.Features.Auth.Queries;

public class GetProfileQuery : IRequest<UserDto>
{
}