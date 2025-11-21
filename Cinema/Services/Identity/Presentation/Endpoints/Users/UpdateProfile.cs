namespace Presentation.Endpoints.Users;

using Application.Abstractions.Messaging;
using Application.Commands.UpdateProfile;
using Extensions;
using Infrastructure;

internal sealed class UpdateProfile : IEndpoint
{
    public sealed record UpdateProfileRequest(
        string? PhoneNumber,
        string? FirstName,
        string? LastName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/me", async (
                UpdateProfileRequest updateProfileRequest,
                ICommandHandler<UpdateProfileCommand, Guid> handler,
                CancellationToken ct) =>
            {
                var command = new UpdateProfileCommand(
                    updateProfileRequest.PhoneNumber,
                    updateProfileRequest.FirstName,
                    updateProfileRequest.LastName);

                var result = await handler.Handle(command, ct);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }
}