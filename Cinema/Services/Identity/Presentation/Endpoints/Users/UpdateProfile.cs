namespace Presentation.Endpoints.Users;

using Routes;
using Application.Commands.UpdateProfile;
using Extensions;
using ErrorHandling;
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

                var result = await mediator.ExecuteCommandAsync<UpdateProfileCommand, Guid>(command, ct);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }
}