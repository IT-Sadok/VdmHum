namespace Shared.Contracts;

using Domain.Enums;

public sealed record BookModel(
    Guid Id,
    string Title,
    List<AuthorModel> Authors,
    int Year,
    BookStatus Status);