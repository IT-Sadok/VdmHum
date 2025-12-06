namespace Presentation.Endpoints.Movies;

using Application.Commands.Movies.DeleteMovie;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class DeleteMovie : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(MoviesRoutes.Delete, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new DeleteMovieCommand(id);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Movies);
    }
}