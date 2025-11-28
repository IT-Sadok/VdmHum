namespace Application.Queries.GetCinema;

using Abstractions.Messaging;
using Contracts.Cinemas;

public sealed record GetCinemaByIdQuery(Guid Id) : IQuery<CinemaResponseModel>;