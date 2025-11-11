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
        book.Year.Value,
        book.Year.IsBC,
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
        var year = dto.IsYearBc
            ? HistoricalYear.BC(dto.Year)
            : HistoricalYear.AD(dto.Year);

        return Book.Rehydrate(dto.Id, title, authors, year, dto.Status);
    }
}