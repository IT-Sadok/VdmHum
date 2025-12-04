namespace Application.Contracts;

public sealed record PagedQuery<TFilter>(
    TFilter Filter,
    int Page,
    int PageSize);