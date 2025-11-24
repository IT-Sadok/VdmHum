namespace Presentation.Endpoints.Users;

using Routes;
using Application.Abstractions.Messaging;
using Application.Commands.CreateAdminUser;
using Extensions;
using Infrastructure;

internal sealed class CreateAdmin : IEndpoint
{
    public sealed record CreateAdminRequest(
        string Email,
        string Password,
        string? PhoneNumber,
        string? FirstName,
        string? LastName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersRoutes.CreateAdmin, async (
                CreateAdminRequest createAdminRequest,
                ICommandHandler<CreateAdminUserCommand, Guid> handler,
                CancellationToken ct) =>
            {
                var command = new CreateAdminUserCommand(
                    createAdminRequest.Email,
                    createAdminRequest.Password,
                    createAdminRequest.PhoneNumber,
                    createAdminRequest.FirstName,
                    createAdminRequest.LastName);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    id => Results.Created($"{UsersRoutes.CreateAdmin}/{id}", new { Id = id }),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Admin);
    }
}