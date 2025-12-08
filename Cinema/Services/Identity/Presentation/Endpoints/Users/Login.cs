namespace Presentation.Endpoints.Users;

using Routes;
using Contracts.Auth;
using Helpers;
using Application.Options;
using Microsoft.Extensions.Options;
using Application.Commands.LoginUser;
using Application.Contracts;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;

internal sealed class Login : IEndpoint
{
    public sealed record LoginRequest(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.Login, async (
                LoginRequest loginRequest,
                HttpContext httpContext,
                IOptions<JwtOptions> jwtOptions,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new LoginUserCommand(loginRequest.Email, loginRequest.Password);

                var result = await mediator.ExecuteCommandAsync<LoginUserCommand, AuthResponseModel>(command, ct);

                return result.Match(
                    auth =>
                    {
                        RefreshTokenCookieHelper.SetRefreshToken(httpContext.Response, auth, jwtOptions.Value);
                        return Results.Ok(new AuthResponse(auth.AccessToken));
                    },
                    CustomResults.Problem);
            })
            .WithTags("Auth");
    }
}