namespace Presentation.Endpoints.Users;

using Application.Options;
using Microsoft.Extensions.Options;
using Contracts.Auth;
using Helpers;
using Application.Abstractions.Messaging;
using Application.Commands.RefreshToken;
using Application.Contracts;
using Extensions;
using Infrastructure;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh", async (
                IOptions<JwtOptions> jwtOptions,
                HttpContext httpContext,
                ICommandHandler<RefreshTokenCommand, AuthResponseModel> handler,
                CancellationToken ct) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Results.Unauthorized();
                }

                var command = new RefreshTokenCommand(refreshToken);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    onSuccess: auth =>
                    {
                        AuthCookieHelper.SetAuthCookies(httpContext.Response, auth, jwtOptions.Value);
                        return Results.Ok(new AuthResponse(auth.UserId));
                    },
                    onFailure: CustomResults.Problem);
            })
            .WithTags(Tags.Auth);
    }
}