namespace Application.Queries.GetHalls;

using Shared.Contracts.Abstractions;
using Contracts.Halls;

public sealed record GetHallsQuery(
    Guid? CinemaId,
    string? Name,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedHallsResponseModel>;