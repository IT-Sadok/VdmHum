namespace Presentation.Endpoints.Users;

using Routes;
using Application.Options;
using Microsoft.Extensions.Options;
using Helpers;
using Application.Commands.LogoutUser;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.Logout, async (
                HttpContext httpContext,
                IOptions<JwtOptions> jwtOptions,
                ICommandHandler<LogoutUserCommand, Result> handler,
                CancellationToken ct) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Results.Unauthorized();
                }

                var command = new LogoutUserCommand(refreshToken);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    onSuccess: () =>
                    {
                        RefreshTokenCookieHelper.ClearRefreshToken(httpContext.Response, jwtOptions.Value);
                        return Results.NoContent();
                    },
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Auth);
    }
}