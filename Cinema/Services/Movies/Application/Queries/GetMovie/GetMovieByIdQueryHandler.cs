namespace Application.Queries.GetMovie;

using Abstractions.Repositories;
using Contracts.Movies;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetMovieByIdQueryHandler(
    IMovieRepository movieRepository)
    : IQueryHandler<GetMovieByIdQuery, MovieResponseModel>
{
    public async Task<Result<MovieResponseModel>> HandleAsync(
        GetMovieByIdQuery query,
        CancellationToken ct)
    {
        var movie = await movieRepository.GetByIdAsync(query.Id, true, ct);

        if (movie is null)
        {
            return Result.Failure<MovieResponseModel>(MovieErrors.NotFound(query.Id));
        }

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