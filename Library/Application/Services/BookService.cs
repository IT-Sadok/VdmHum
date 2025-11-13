namespace Application.Services;

using Interfaces;
using Mappers;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions.Book;
using Domain.ValueObjects;
using Shared.Contracts;

public sealed class BookService(IBookRepository repository) : IBookService
{
    public async Task<BookModel> GetBookByIdAsync(Guid id, CancellationToken ct = default)
    {
        var book = await repository.GetByIdAsync(id, ct)
                   ?? throw new BookNotFoundException(id);

        return BookMapper.ToModel(book);
    }

    public async Task<IEnumerable<BookModel>> SearchAsync(string query, CancellationToken ct = default)
    {
        query = query.Trim();
        var all = await repository.GetAllAsync(ct);

        var matches = all.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
            || b.Authors.Any(a => a.Name != null &&
                                  a.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));

        return matches.Select(BookMapper.ToModel).ToList();
    }

    public async Task<IEnumerable<BookModel>> GetAllAsync(CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);

        return all.Select(BookMapper.ToModel).ToList();
    }

    public async Task<IEnumerable<BookModel>> GetAllAvailableAsync(CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);
        var available = all.Where(b => b.Status == BookStatus.Available);

        return available.Select(BookMapper.ToModel).ToList();
    }

    public async Task AddBookAsync(BookUpsertModel model, CancellationToken ct = default)
    {
        if (model.Authors is null || model.Authors.Count == 0)
        {
            throw new InvalidOperationException("Authors cannot be empty.");
        }

        var authorObjects = model.Authors.Select(MapModelToValueObject).ToHashSet();

        var book = Book.Create(model.Title, authorObjects, new DateOnly(model.Year, 1, 1));

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

    private static Author MapModelToValueObject(AuthorModel model) =>
        model.Type switch
        {
            AuthorType.Known => Author.Known(model.Name!),
            AuthorType.Pseudonym => Author.Pseudonym(model.Name!),
            AuthorType.Anonymous => Author.Anonymous(),
            AuthorType.Folk => Author.Folk(),
            AuthorType.Unknown => Author.Unknown(),
            _ => throw new InvalidOperationException($"Invalid author type: {model.Type}")
        };
}