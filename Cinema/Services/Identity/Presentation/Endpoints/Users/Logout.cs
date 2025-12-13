namespace Presentation.Endpoints.Users;

using Routes;
using Application.Options;
using Microsoft.Extensions.Options;
using Helpers;
using Application.Commands.LogoutUser;
using Extensions;
using ErrorHandling;
using Shared.Contracts.Abstractions;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.Logout, async (
                HttpContext httpContext,
                IOptions<JwtOptions> jwtOptions,
                IMediator mediator,
                CancellationToken ct) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Results.Unauthorized();
                }

                var command = new LogoutUserCommand(refreshToken);

                var result = await mediator.ExecuteCommandAsync(command, ct);

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