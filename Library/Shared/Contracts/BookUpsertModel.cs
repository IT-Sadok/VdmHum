namespace Shared.Contracts;

using Domain.Enums;

public sealed record BookUpsertModel(
    string Title,
    List<AuthorModel> Authors,
    int Year,
    BookStatus Status);