namespace Presentation.Endpoints.Users;

using Application.Options;
using Microsoft.Extensions.Options;
using Helpers;
using Application.Abstractions.Messaging;
using Application.Commands.LogoutUser;
using Domain.Abstractions;
using Extensions;
using Infrastructure;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/logout", async (
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

                var result = await handler.Handle(command, ct);

                return result.Match(
                    onSuccess: () =>
                    {
                        AuthCookieHelper.ClearAuthCookies(httpContext.Response, jwtOptions.Value);
                        return Results.NoContent();
                    },
                    onFailure: CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Auth);
    }
}