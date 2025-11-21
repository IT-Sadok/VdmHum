namespace Presentation.Endpoints.Users;

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
        app.MapPost("admin/users", async (
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

                var result = await handler.Handle(command, ct);

                return result.Match(
                    id => Results.Created($"/admin/users/{id}", new { Id = id }),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Admin);
    }
}