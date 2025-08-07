using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Models.Auth;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class LoginCommandHandler(IUserRepository _userRepository, IJwtService _jwtService, IMapper _mapper)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by username or email
        var user = await _userRepository.GetByEmailAsync(request.Username);

        if (user == null || user.DeletedAt.HasValue)
        {
            throw new UnauthenticatedException("Invalid username or password");
        }

        var isPasswordValid = new PasswordHasher<UserEntity>()
            .VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Failed;

        // Verify password
        if (!isPasswordValid)
        {
            throw new BadRequestException("Invalid username or password");
        }

        // Generate tokens
        var accessToken = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        
        

        // Update user with refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(365);
        
        await _userRepository.UpdateAsync(user);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email
        };
    }
}