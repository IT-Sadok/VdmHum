namespace Application.Commands.Movies.CreateMovie;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Application.Contracts.Movies;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;

public sealed class CreateMovieCommandHandler(
    IMovieRepository movieRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMovieCommand, MovieResponseModel>
{
    public async Task<Result<MovieResponseModel>> HandleAsync(
        CreateMovieCommand command,
        CancellationToken ct)
    {
        var isTitleUnique = await movieRepository.IsTitleUniqueAsync(
            command.Title,
            excludeMovieId: null,
            ct);

        if (!isTitleUnique)
        {
            return Result.Failure<MovieResponseModel>(MovieErrors.TitleNotUnique);
        }

        var movie = Movie.Create(
            title: command.Title,
            status: command.Status,
            description: command.Description,
            genres: command.Genres,
            durationMinutes: command.DurationMinutes,
            ageRating: command.AgeRating,
            releaseDate: command.ReleaseDate,
            posterUrl: command.PosterUrl);

        movieRepository.Add(movie, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var response = new MovieResponseModel(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.MovieGenres.Select(mg => mg.Genre).ToArray(),
            movie.DurationMinutes,
            movie.AgeRating,
            movie.Status,
            movie.ReleaseDate,
            movie.PosterUrl);

        return response;
    }
}