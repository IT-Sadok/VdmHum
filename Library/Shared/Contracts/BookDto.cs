namespace Shared.Contracts;

using Domain.Enums;

public sealed record BookDto(
    Guid Id,
    string Title,
    List<AuthorDto> Authors,
    int Year,
    BookStatus Status);