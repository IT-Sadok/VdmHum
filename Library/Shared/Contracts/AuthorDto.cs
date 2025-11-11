namespace Shared.Contracts;

using Domain.Enums;

public sealed record AuthorDto(
    string? Name,
    AuthorType Type);