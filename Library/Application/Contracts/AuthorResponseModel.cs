namespace Application.Contracts;

using Domain.Enums;

public sealed record AuthorResponseModel(string? Name, AuthorType Type);