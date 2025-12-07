namespace Application.Contracts;

public sealed record PagedFilter<TFilter>(
    TFilter ModelFilter,
    int Page,
    int PageSize);