namespace Application.Contracts;

using Domain.Enums;

public sealed record BookResponseModel(
    Guid Id,
    string Title,
    List<AuthorResponseModel> Authors,
    int Year,
    BookStatus Status);