namespace Presentation.Endpoints.Users;

using Routes;
using Application.Options;
using Microsoft.Extensions.Options;
using Contracts.Auth;
using Helpers;
using Application.Commands.RefreshToken;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.Refresh, async (
                IOptions<JwtOptions> jwtOptions,
                HttpContext httpContext,
                IMediator mediator,
                CancellationToken ct) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Results.Unauthorized();
                }

                var command = new RefreshTokenCommand(refreshToken);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    onSuccess: auth =>
                    {
                        RefreshTokenCookieHelper.SetRefreshToken(httpContext.Response, auth, jwtOptions.Value);
                        return Results.Ok(new AuthResponse(auth.AccessToken));
                    },
                    onFailure: CustomResults.Problem);
            })
            .WithTags(Tags.Auth);
    }
}