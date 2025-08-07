using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Models.Auth;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Auth.Commands;

public class VerifyOtpCommandHandler(
    IOtpService _otpService,
    IMapper _mapper,
    IJwtService _jwtService,
    IUserRepository _userRepository,
    IRoleRepository _roleRepository
)
    : IRequestHandler<VerifyOtpCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var lowerCaseEmail = request.Email.ToLower();
        var isValid = _otpService.VerifyOtp(lowerCaseEmail, request.Otp);
        if (!isValid)
        {
            throw new BadRequestException("Invalid otp or otp Expired!");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email);

        var accessToken = "";
        var refreshToken = "";

        if (user is null)
        {
            var defaultRole = await _roleRepository.GetByNameAsync(RoleName.Guest);
            // Create new user
            var newUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = request.Email,
                Email = request.Email,
                PasswordHash = new PasswordHasher<UserEntity>().HashPassword(new UserEntity()
                {
                    Username = request.Email,
                    Email = request.Email,
                    Id = Guid.NewGuid(),
                }, DateTime.UtcNow.ToString()),
                CreatedBy = request.Email,
            };

            if (defaultRole != null)
            {
                newUser.UserRoles.Add(new UserRoleEntity()
                {
                    UserId = newUser.Id,
                    RoleId = defaultRole.Id,
                    User = newUser,
                    Role = defaultRole
                });
            }


            // Generate tokens
            accessToken = _jwtService.GenerateToken(newUser);
            refreshToken = _jwtService.GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(365);

            await _userRepository.AddAsync(newUser);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email
            };
        }

        // Generate tokens
        accessToken = _jwtService.GenerateToken(user);
        refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(365);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email
        };
    }
}