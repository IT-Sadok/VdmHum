namespace Shared.Contracts;

using Domain.Enums;

public sealed record BookUpsertDto(
    string Title,
    List<AuthorDto> Authors,
    int Year,
    bool IsYearBc,
    BookStatus Status);