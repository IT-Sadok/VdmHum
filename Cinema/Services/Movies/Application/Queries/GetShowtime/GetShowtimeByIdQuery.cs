namespace Application.Queries.GetShowtime;

using Shared.Contracts.Abstractions;
using Contracts.Showtimes;

public sealed record GetShowtimeByIdQuery(Guid Id) : IQuery<ShowtimeResponseModel>;