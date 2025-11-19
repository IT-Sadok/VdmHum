namespace Application.Contracts;

public sealed record BookUpsertModel(
    string Title,
    List<AuthorUpsertModel> Authors,
    int Year);