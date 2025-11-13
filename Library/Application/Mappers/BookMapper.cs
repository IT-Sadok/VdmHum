namespace Application.Mappers;

using System.Text.Json;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Shared.Contracts;

public static class BookMapper
{
    public static BookModel ToModel(Book book) => new(
        book.Id,
        book.Title,
        book.Authors.Select(a => new AuthorModel(a.Name, a.Type)).ToList(),
        book.Year.Year,
        book.Status);

    public static Book FromModel(BookModel model)
    {
        var authors = model.Authors.Select(a =>
            a.Type switch
            {
                AuthorType.Known => Author.Known(a.Name!),
                AuthorType.Pseudonym => Author.Pseudonym(a.Name!),
                AuthorType.Anonymous => Author.Anonymous(),
                AuthorType.Folk => Author.Folk(),
                AuthorType.Unknown => Author.Unknown(),
                _ => throw new JsonException($"Invalid author type {a.Type}")
            }).ToHashSet();

        return Book.Rehydrate(
            model.Id, 
            model.Title, 
            authors, 
            new DateOnly(model.Year, 1, 1) , 
            model.Status);
    }
}