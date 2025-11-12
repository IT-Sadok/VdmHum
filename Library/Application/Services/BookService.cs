namespace Application.Services;

using Interfaces;
using Mappers;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Author;
using Domain.Exceptions.Book;
using Domain.ValueObjects;
using Shared.Contracts;

public sealed class BookService(IBookRepository repository) : IBookService
{
    public async Task<BookDto> GetBookByIdAsync(Guid id, CancellationToken ct = default)
    {
        var book = await repository.GetByIdAsync(id, ct)
                   ?? throw new BookNotFoundException(id);

        return BookMapper.ToDto(book);
    }
    
    public async Task<IEnumerable<BookDto>> SearchAsync(string query, CancellationToken ct = default)
    {
        query = query.Trim();
        var all = await repository.GetAllAsync(ct);

        var matches = all.Where(b =>
            b.Title.Value.Contains(query, StringComparison.OrdinalIgnoreCase)
            || b.Authors.Any(a => a.Name != null &&
                                  a.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));

        return matches.Select(BookMapper.ToDto).ToList();
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);

        return all.Select(BookMapper.ToDto).ToList();
    }
    
    public async Task<IEnumerable<BookDto>> GetAllAvailableAsync(CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);
        var available = all.Where(b => b.Status == BookStatus.Available);

        return available.Select(BookMapper.ToDto).ToList();
    }

    public async Task AddBookAsync(BookUpsertDto dto, CancellationToken ct = default)
    {
        if (dto.Authors is null || dto.Authors.Count == 0)
        {
            throw new EmptyAuthorsListException();
        }

        var authorObjects = dto.Authors.Select(MapDtoToValueObject).ToList();

        var bookTitle = BookTitle.Create(dto.Title);

        var year = dto.IsYearBc
            ? HistoricalYear.BC(dto.Year)
            : HistoricalYear.AD(dto.Year);

        var book = Book.Create(bookTitle, authorObjects, year);

        await repository.AddAsync(book, ct);
    }

    public async Task DeleteBookAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await repository.GetByIdAsync(id, ct);
        if (existing is null)
        {
            throw new BookNotFoundException(id);
        }

        await repository.DeleteAsync(id, ct);
    }

    public async Task BorrowBookAsync(Guid id, CancellationToken ct = default)
    {
        var book = await repository.GetByIdAsync(id, ct)
                   ?? throw new BookNotFoundException(id);

        book.Borrow();

        await repository.UpdateAsync(book, ct);
    }

    public async Task ReturnBookAsync(Guid id, CancellationToken ct = default)
    {
        var book = await repository.GetByIdAsync(id, ct)
                   ?? throw new BookNotFoundException(id);

        book.Return();

        await repository.UpdateAsync(book, ct);
    }

    private static Author MapDtoToValueObject(AuthorDto dto) =>
        dto.Type switch
        {
            AuthorType.Known => Author.Known(dto.Name!),
            AuthorType.Pseudonym => Author.Pseudonym(dto.Name!),
            AuthorType.Anonymous => Author.Anonymous(),
            AuthorType.Folk => Author.Folk(),
            AuthorType.Unknown => Author.Unknown(),
            _ => throw new InvalidAuthorTypeException(dto.Type.ToString())
        };
}