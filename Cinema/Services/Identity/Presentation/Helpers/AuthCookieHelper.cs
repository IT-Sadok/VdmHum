namespace Presentation.Helpers;

using Application.Contracts;
using Application.Options;

public static class AuthCookieHelper
{
    public static void SetAuthCookies(HttpResponse response, AuthResponseModel auth, JwtOptions jwt)
    {
        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddMinutes(jwt.AccessTokenLifetimeMinutes),
        };

        response.Cookies.Append(jwt.AccessTokenName, auth.AccessToken, accessCookieOptions);

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/auth",
            Expires = DateTimeOffset.UtcNow.AddDays(jwt.RefreshTokenLifetimeDays),
        };

        response.Cookies.Append(jwt.RefreshTokenName, auth.RefreshToken, refreshCookieOptions);
    }

    public static void ClearAuthCookies(HttpResponse response, JwtOptions jwt)
    {
        var accessExpired = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
        };
        response.Cookies.Append(jwt.AccessTokenName, string.Empty, accessExpired);

        var refreshExpired = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/auth",
        };
        response.Cookies.Append(jwt.RefreshTokenName, string.Empty, refreshExpired);
    }
}