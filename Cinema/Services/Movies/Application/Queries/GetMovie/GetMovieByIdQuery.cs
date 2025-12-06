namespace Application.Queries.GetMovie;

using Shared.Contracts.Abstractions;
using Contracts.Movies;

public sealed record GetMovieByIdQuery(Guid Id) : IQuery<MovieResponseModel>;