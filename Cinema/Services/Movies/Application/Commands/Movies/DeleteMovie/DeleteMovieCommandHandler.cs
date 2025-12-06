namespace Application.Commands.Movies.DeleteMovie;

using Abstractions.Repositories;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class DeleteMovieCommandHandler(
    IMovieRepository movieRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMovieCommand>
{
    public async Task<Result> HandleAsync(
        DeleteMovieCommand command,
        CancellationToken ct)
    {
        var movie = await movieRepository.GetByIdAsync(command.Id, false, ct);

        if (movie is null)
        {
            return Result.Failure(MovieErrors.NotFound(command.Id));
        }

        movieRepository.Remove(movie);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}