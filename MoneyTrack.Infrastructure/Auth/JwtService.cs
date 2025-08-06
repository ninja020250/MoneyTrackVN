using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Infrastructure.Auth;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JwtSettings:SecretKey"] ??
                     throw new ArgumentNullException("JWT SecretKey not configured");
        _issuer = _configuration["JwtSettings:Issuer"] ?? "MoneyTrackApi";
        _audience = _configuration["JwtSettings:Audience"] ?? "MoneyTrackClient";
        _accessTokenExpirationMinutes = int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]);
    }

    public string GenerateToken(UserEntity user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            }.Concat(user.UserRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole.Role.Name.ToString())))
            .ToList();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddMinutes(_accessTokenExpirationMinutes));

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public bool ValidateRefreshToken(string refreshToken, UserEntity user)
    {
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return false;
        return true;
    }

    public string GetEmailFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.ReadJwtToken(token);
        return jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    }
}