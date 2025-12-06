namespace Application.Queries.GetCinemas;

using Shared.Contracts.Abstractions;
using Contracts.Cinemas;

public sealed record GetCinemasQuery(
    string? Name,
    string? City,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedCinemasResponseModel>;