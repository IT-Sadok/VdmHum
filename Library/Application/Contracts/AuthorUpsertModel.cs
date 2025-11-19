namespace Application.Contracts;

using Domain.Enums;

public sealed record AuthorUpsertModel(string? Name, AuthorType Type);