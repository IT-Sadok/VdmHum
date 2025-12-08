namespace Presentation.Endpoints.Cinemas;

using Application.Commands.Cinemas.DeleteCinema;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class DeleteCinema : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(CinemasRoutes.Delete, async (
                [FromRoute] Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new DeleteCinemaCommand(id);

                var result = await mediator.ExecuteCommandAsync(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Cinemas);
    }
}