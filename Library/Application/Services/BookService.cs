namespace Application.Services;

using Mappers;
using Contracts;
using Interfaces;
using Domain.Enums;
using Domain.Exceptions.Book;

public sealed class BookService(IBookRepository repository) : IBookService
{
    public async Task<BookResponseModel> GetBookByIdAsync(Guid id, CancellationToken ct = default)
    {
        var book = await repository.GetByIdAsync(id, ct)
                   ?? throw new BookNotFoundException(id);

        return BookModelMapper.ToResponse(book);
    }

    public async Task<IEnumerable<BookResponseModel>> SearchAsync(string query, CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);
        query = query.Trim();

        var matches = all.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
            || b.Authors.Any(a => a.Name != null &&
                                  a.Name.Contains(query, StringComparison.OrdinalIgnoreCase)));

        return matches.Select(BookModelMapper.ToResponse).ToList();
    }

    public async Task<IEnumerable<BookResponseModel>> GetAllAsync(CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);

        return all.Select(BookModelMapper.ToResponse).ToList();
    }

    public async Task<IEnumerable<BookResponseModel>> GetAllAvailableAsync(CancellationToken ct = default)
    {
        var all = await repository.GetAllAsync(ct);
        var available = all.Where(b => b.Status == BookStatus.Available);

        return available.Select(BookModelMapper.ToResponse).ToList();
    }

    public async Task AddBookAsync(BookUpsertModel model, CancellationToken ct = default)
    {
        if (model.Authors is null || model.Authors.Count == 0)
        {
            throw new InvalidOperationException("Authors cannot be empty.");
        }

        var book = BookModelMapper.ToDomain(model);
        await repository.AddAsync(book, ct);
    }

    public async Task DeleteBookAsync(Guid id, CancellationToken ct = default)
    {
        _ = await repository.GetByIdAsync(id, ct) 
            ?? throw new BookNotFoundException(id);

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
}