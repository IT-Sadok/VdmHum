namespace Infrastructure.Mappers;

using Contracts;
using System.Text.Json;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

public static class BookStorageMapper
{
    public static BookStorageModel ToStorageModel(Book book) => new(
        book.Id,
        book.Title,
        book.Authors
            .Select(a => new AuthorStorageModel(a.Name, a.Type))
            .ToList(),
        book.Year.Year,
        book.Status);

    public static Book FromStorageModel(BookStorageModel model)
    {
        var authors = model.Authors
            .Select(a => a.Type switch
            {
                AuthorType.Known => Author.Known(a.Name!),
                AuthorType.Pseudonym => Author.Pseudonym(a.Name!),
                AuthorType.Anonymous => Author.Anonymous(),
                AuthorType.Folk => Author.Folk(),
                AuthorType.Unknown => Author.Unknown(),
                _ => throw new JsonException($"Invalid author type {a.Type}")
            })
            .ToHashSet();

        return Book.Rehydrate(
            model.Id,
            model.Title,
            authors,
            new DateOnly(model.Year, 1, 1),
            model.Status);
    }
}