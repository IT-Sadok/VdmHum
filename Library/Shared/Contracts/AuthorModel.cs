namespace Shared.Contracts;

using Domain.Enums;

public sealed record AuthorModel(
    string? Name,
    AuthorType Type);