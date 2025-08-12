using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MoneyTrack.Application.Contracts.Persistence;

namespace MoneyTrack.Infrastructure.Auth;

public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return new Guid(userIdClaim);
        }
    }
}