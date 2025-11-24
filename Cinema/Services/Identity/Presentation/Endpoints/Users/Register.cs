namespace Presentation.Endpoints.Users;

using Application.Options;
using Microsoft.Extensions.Options;
using Contracts.Auth;
using Helpers;
using Application.Abstractions.Messaging;
using Application.Commands.RegisterUser;
using Application.Contracts;
using Extensions;
using Infrastructure;

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
        app.MapPost("auth/register", async (
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
                        AuthCookieHelper.SetAuthCookies(httpContext.Response, auth, jwtOptions.Value);
                        return Results.Ok(new AuthResponse(auth.UserId));
                    },
                    CustomResults.Problem);
            })
            .WithTags(Tags.Auth);
    }
}