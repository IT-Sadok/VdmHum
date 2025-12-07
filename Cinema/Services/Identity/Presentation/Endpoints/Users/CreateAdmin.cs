namespace Presentation.Endpoints.Users;

using Routes;
using Application.Commands.CreateAdminUser;
using Application.Contracts;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;

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
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new CreateAdminUserCommand(
                    createAdminRequest.Email,
                    createAdminRequest.Password,
                    createAdminRequest.PhoneNumber,
                    createAdminRequest.FirstName,
                    createAdminRequest.LastName);

                var result = await mediator.ExecuteCommandAsync
                    <CreateAdminUserCommand, CreateAdminResponseModel>(command, ct);

                return result.Match(
                    adminResponseModel => Results.Created(
                        UsersRoutes.GetCurrent,
                        adminResponseModel),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Admin);
    }
}