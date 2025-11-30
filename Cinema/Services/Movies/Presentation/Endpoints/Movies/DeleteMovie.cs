namespace Presentation.Endpoints.Movies;

using Application.Abstractions.Messaging;
using Application.Commands.Movies.DeleteMovie;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class DeleteMovie : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(MoviesRoutes.Delete, async (
                Guid id,
                ICommandHandler<DeleteMovieCommand> handler,
                CancellationToken ct) =>
            {
                var command = new DeleteMovieCommand(id);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Movies);
    }
}