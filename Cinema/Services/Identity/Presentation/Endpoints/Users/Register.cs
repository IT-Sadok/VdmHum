namespace Presentation.Endpoints.Users;

using Routes;
using Application.Options;
using Microsoft.Extensions.Options;
using Contracts.Auth;
using Helpers;
using Application.Commands.RegisterUser;
using Application.Contracts;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;

internal sealed class Register : IEndpoint
{
    public sealed record RegisterRequest(
        string Email,
        string Password,
        string? PhoneNumber,
        string? FirstName,
        string? LastName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.Register, async (
                RegisterRequest registerRequest,
                IOptions<JwtOptions> jwtOptions,
                HttpContext httpContext,
                ICommandHandler<RegisterUserCommand, AuthResponseModel> handler,
                CancellationToken ct) =>
            {
                var command = new RegisterUserCommand(
                    registerRequest.Email,
                    registerRequest.Password,
                    registerRequest.PhoneNumber,
                    registerRequest.FirstName,
                    registerRequest.LastName);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    auth =>
                    {
                        RefreshTokenCookieHelper.SetRefreshToken(httpContext.Response, auth, jwtOptions.Value);
                        return Results.Ok(new AuthResponse(auth.AccessToken));
                    },
                    CustomResults.Problem);
            })
            .WithTags(Tags.Auth);
    }
}