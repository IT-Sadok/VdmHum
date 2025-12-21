namespace Infrastructure.Services;

using System.Security.Claims;
using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Http;

public class UserContextService(IHttpContextAccessor httpContextAccessor)
    : IUserContextService
{
    public UserContextModel GetUserContext()
    {
        var httpUser = httpContextAccessor.HttpContext?.User;

        var idStr = httpUser?.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = Guid.TryParse(idStr, out var id) ? id : null;

        bool isAuthenticated = httpUser?.Identity?.IsAuthenticated ?? false;

        return new UserContextModel(userId, isAuthenticated);
    }
}