namespace Application.Queries.GetMovies;

using Abstractions.Repositories;
using Contracts;
using Contracts.Movies;
using Domain.Entities;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetMoviesQueryHandler(
    IMovieRepository movieRepository)
    : IQueryHandler<GetMoviesQuery, PagedResponse<MovieResponseModel>>
{
    public async Task<Result<PagedResponse<MovieResponseModel>>> HandleAsync(
        GetMoviesQuery query,
        CancellationToken ct)
    {
        var (items, totalCount) = await movieRepository.GetPagedAsync(
            query.PagedFilter,
            ct);

        var responseItems = items
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedResponse<MovieResponseModel>(
            Page: query.PagedFilter.Page,
            PageSize: query.PagedFilter.PageSize,
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