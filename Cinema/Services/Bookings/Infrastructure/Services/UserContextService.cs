namespace Infrastructure.Services;

using System.Security.Claims;
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

public class UserContextService(IHttpContextAccessor httpContextAccessor)
    : IUserContextService
{
    public Guid? UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userId, out var id) ? id : null;
        }
    }

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}