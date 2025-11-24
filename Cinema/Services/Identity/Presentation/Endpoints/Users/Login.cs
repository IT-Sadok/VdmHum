namespace Presentation.Endpoints.Users;

using Routes;
using Contracts.Auth;
using Helpers;
using Application.Options;
using Microsoft.Extensions.Options;
using Application.Abstractions.Messaging;
using Application.Commands.LoginUser;
using Application.Contracts;
using Extensions;
using Infrastructure;

internal sealed class Login : IEndpoint
{
    public sealed record LoginRequest(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.Login, async (
                LoginRequest loginRequest,
                HttpContext httpContext,
                IOptions<JwtOptions> jwtOptions,
                ICommandHandler<LoginUserCommand, AuthResponseModel> handler,
                CancellationToken ct) =>
            {
                var command = new LoginUserCommand(loginRequest.Email, loginRequest.Password);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    auth =>
                    {
                        AuthCookieHelper.SetAuthCookies(httpContext.Response, auth, jwtOptions.Value);
                        return Results.Ok(new AuthResponse(auth.UserId));
                    },
                    CustomResults.Problem);
            })
            .WithTags("Auth");
    }
}