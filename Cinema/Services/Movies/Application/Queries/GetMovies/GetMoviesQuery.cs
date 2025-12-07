namespace Application.Queries.GetMovies;

using Contracts;
using Shared.Contracts.Abstractions;
using Contracts.Movies;

public sealed record GetMoviesQuery(
    PagedFilter<MovieFilter> PagedFilter
) : IQuery<PagedResponse<MovieResponseModel>>;