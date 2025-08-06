using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Models.Auth;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class RegisterCommandHandler(
    IUserRepository _userRepository,
    IJwtService _jwtService,
    IMapper _mapper
) : IRequestHandler<RegisterCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new BadRequestException("User with this email already exists");
        }

        // Create new user
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = new PasswordHasher<UserEntity>().HashPassword(new UserEntity()
            {
                Username = request.Username,
                Email = request.Email,
                Id = Guid.NewGuid(),
            }, request.Password),
            CreatedDate = DateTime.UtcNow,
            CreatedBy = request.Username
        };

        // Generate tokens
        var accessToken = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(365);
  
        await _userRepository.AddAsync(user);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email
        };
    }
}