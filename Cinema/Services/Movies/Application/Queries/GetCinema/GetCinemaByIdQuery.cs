namespace Application.Queries.GetCinema;

using Contracts.Cinemas;
using Shared.Contracts.Abstractions;

public sealed record GetCinemaByIdQuery(Guid Id) : IQuery<CinemaResponseModel>;