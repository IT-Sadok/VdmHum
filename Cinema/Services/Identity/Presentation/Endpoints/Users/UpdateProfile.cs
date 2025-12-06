namespace Presentation.Endpoints.Users;

using Routes;
using Application.Commands.UpdateProfile;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;

internal sealed class UpdateProfile : IEndpoint
{
    public sealed record UpdateProfileRequest(
        string? PhoneNumber,
        string? FirstName,
        string? LastName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UsersRoutes.UpdateProfile, async (
                UpdateProfileRequest updateProfileRequest,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new UpdateProfileCommand(
                    updateProfileRequest.PhoneNumber,
                    updateProfileRequest.FirstName,
                    updateProfileRequest.LastName);

                var result = await mediator.Send(command, ct);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }
}