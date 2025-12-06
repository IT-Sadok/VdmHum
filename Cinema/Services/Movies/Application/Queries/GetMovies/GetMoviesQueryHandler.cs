namespace Application.Queries.GetMovies;

using Abstractions.Repositories;
using Contracts.Movies;
using Domain.Entities;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetMoviesQueryHandler(
    IMovieRepository movieRepository)
    : IQueryHandler<GetMoviesQuery, PagedMoviesResponseModel>
{
    public async Task<Result<PagedMoviesResponseModel>> HandleAsync(
        GetMoviesQuery query,
        CancellationToken ct)
    {
        var filter = new MovieFilter(
            Genres: query.Genres,
            MinDurationMinutes: query.MinDurationMinutes,
            MaxDurationMinutes: query.MaxDurationMinutes,
            MinAgeRating: query.MinAgeRating,
            MaxAgeRating: query.MaxAgeRating,
            Status: query.Status);

        var (items, totalCount) = await movieRepository.GetPagedAsync(
            filter,
            query.Page,
            query.PageSize,
            ct);

        var responseItems = items
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedMoviesResponseModel(
            Page: query.Page,
            PageSize: query.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }

    private static MovieResponseModel MapToResponse(Movie movie) =>
        new(
            movie.Id,
            movie.Title,
            movie.Description,
            movie.MovieGenres.Select(mg => mg.Genre).ToArray(),
            movie.DurationMinutes,
            movie.AgeRating,
            movie.Status,
            movie.ReleaseDate,
            movie.PosterUrl);
}