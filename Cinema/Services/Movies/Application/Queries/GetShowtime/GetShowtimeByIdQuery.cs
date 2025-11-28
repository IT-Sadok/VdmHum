namespace Application.Queries.GetShowtime;

using Abstractions.Messaging;
using Contracts.Showtimes;

public sealed record GetShowtimeByIdQuery(Guid Id) : IQuery<ShowtimeResponseModel>;