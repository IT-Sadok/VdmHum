namespace Application.Queries.GetMovie;

using Abstractions.Messaging;
using Contracts.Movies;

public sealed record GetMovieByIdQuery(Guid Id) : IQuery<MovieResponseModel>;