namespace Application.Queries.GetShowtimes;

using Shared.Contracts.Abstractions;
using Contracts.Showtimes;
using Domain.Enums;

public sealed record GetShowtimesQuery(
    Guid? MovieId,
    Guid? CinemaId,
    Guid? HallId,
    DateTime? DateFromUtc,
    DateTime? DateToUtc,
    ShowtimeStatus? Status,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedShowtimesResponseModel>;