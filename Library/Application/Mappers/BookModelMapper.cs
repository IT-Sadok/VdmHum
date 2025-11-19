namespace Application.Mappers;

using Contracts;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

public static class BookModelMapper
{
    public static BookResponseModel ToResponse(Book book) => new(
        book.Id,
        book.Title,
        book.Authors
            .Select(a => new AuthorResponseModel(a.Name, a.Type))
            .ToList(),
        book.Year.Year,
        book.Status);

    public static Book ToDomain(BookUpsertModel model) =>
        Book.Create(
            model.Title,
            ToDomainAuthors(model.Authors),
            new DateOnly(model.Year, 1, 1));

    private static HashSet<Author> ToDomainAuthors(IEnumerable<AuthorUpsertModel> models) =>
        models.Select(m => m.Type switch
        {
            AuthorType.Known => Author.Known(m.Name!),
            AuthorType.Pseudonym => Author.Pseudonym(m.Name!),
            AuthorType.Anonymous => Author.Anonymous(),
            AuthorType.Folk => Author.Folk(),
            AuthorType.Unknown => Author.Unknown(),
            _ => throw new InvalidOperationException($"Invalid author type: {m.Type}")
        }).ToHashSet();
}