namespace Application.Mappers;

using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Shared.Contracts;

public static class BookMapper
{
    public static BookDto ToDto(Book book) => new(
        book.Id,
        book.Title.Value,
        book.Authors.Select(a => new AuthorDto(a.Name ?? "N/A", a.Type)).ToList(),
        book.Year.Year,
        book.Status);

    public static Book FromDto(BookDto dto)
    {
        var authors = dto.Authors.Select(a =>
            a.Type switch
            {
                AuthorType.Known => Author.Known(a.Name ?? "N/A"),
                AuthorType.Pseudonym => Author.Pseudonym(a.Name ?? "N/A"),
                AuthorType.Anonymous => Author.Anonymous(),
                AuthorType.Folk => Author.Folk(),
                AuthorType.Unknown => Author.Unknown(),
                _ => throw new ArgumentOutOfRangeException()
            }).ToList();

        var title = BookTitle.Create(dto.Title);

        return Book.Rehydrate(dto.Id, title, authors, new DateOnly(dto.Year, 1, 1) , dto.Status);
    }
}