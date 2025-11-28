namespace Application.Commands.Movies.UpdateMovie;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Movies;
using Domain.Abstractions;
using Domain.Errors;

public sealed class UpdateMovieCommandHandler(
    IMovieRepository movieRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMovieCommand, MovieResponseModel>
{
    public async Task<Result<MovieResponseModel>> HandleAsync(
        UpdateMovieCommand command,
        CancellationToken ct)
    {
        var movie = await movieRepository.GetByIdAsync(command.Id, ct);

        if (movie is null)
        {
            return Result.Failure<MovieResponseModel>(MovieErrors.NotFound(command.Id));
        }

        var isTitleUnique = await movieRepository.IsTitleUniqueAsync(
            command.Title,
            excludeMovieId: command.Id,
            ct);

        if (!isTitleUnique)
        {
            return Result.Failure<MovieResponseModel>(MovieErrors.TitleNotUnique);
        }

        movie.UpdateTitle(command.Title);
        movie.ChangeStatus(command.Status);
        movie.UpdateDescription(command.Description);
        movie.UpdateDuration(command.DurationMinutes);
        movie.UpdatePosterUrl(command.PosterUrl);

        if (command.AgeRating is not null)
        {
            movie.UpdateAgeRating(command.AgeRating.Value);
        }

        if (command.ReleaseDate is not null)
        {
            movie.UpdateReleaseDate(command.ReleaseDate.Value);
        }

        if (command.Genres is not null)
        {
            movie.SetGenres(command.Genres);
        }

        await unitOfWork.SaveChangesAsync(ct);

        var response = new MovieResponseModel(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.Genres.ToArray(),
            movie.DurationMinutes,
            movie.AgeRating,
            movie.Status,
            movie.ReleaseDate,
            movie.PosterUrl);

        return response;
    }
}