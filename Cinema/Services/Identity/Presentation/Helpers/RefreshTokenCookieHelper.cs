namespace Presentation.Helpers;

using Endpoints.Routes;
using Application.Contracts;
using Application.Options;

public static class RefreshTokenCookieHelper
{
    public static void SetRefreshToken(HttpResponse response, AuthResponseModel auth, JwtOptions jwt)
    {
        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = UsersRoutes.Refresh,
            Expires = DateTimeOffset.UtcNow.AddDays(jwt.RefreshTokenLifetimeDays),
        };

        response.Cookies.Append(jwt.RefreshTokenName, auth.RefreshToken, refreshCookieOptions);
    }

    public static void ClearRefreshToken(HttpResponse response, JwtOptions jwt)
    {
        var refreshExpired = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = UsersRoutes.Refresh,
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
        };
        response.Cookies.Append(jwt.RefreshTokenName, string.Empty, refreshExpired);
    }
}