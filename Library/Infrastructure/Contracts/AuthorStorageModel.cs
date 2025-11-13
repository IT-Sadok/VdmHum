namespace Infrastructure.Contracts;

using Domain.Enums;

public sealed record AuthorStorageModel(
    string? Name,
    AuthorType Type);