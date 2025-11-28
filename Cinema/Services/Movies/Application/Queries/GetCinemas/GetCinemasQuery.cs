namespace Application.Queries.GetCinemas;

using Abstractions.Messaging;
using Contracts.Cinemas;

public sealed record GetCinemasQuery(
    string? Name,
    string? City,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedCinemasResponseModel>;