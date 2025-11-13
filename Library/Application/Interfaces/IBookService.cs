using Shared.Contracts;

namespace Application.Interfaces;

public interface IBookService
{
    Task<BookModel> GetBookByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<BookModel>> SearchAsync(string query, CancellationToken ct = default);
    Task<IEnumerable<BookModel>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<BookModel>> GetAllAvailableAsync(CancellationToken ct = default);
    Task AddBookAsync(BookUpsertModel model, CancellationToken ct = default);
    Task DeleteBookAsync(Guid id, CancellationToken ct = default);
    Task BorrowBookAsync(Guid id, CancellationToken ct = default);
    Task ReturnBookAsync(Guid id, CancellationToken ct = default);
}