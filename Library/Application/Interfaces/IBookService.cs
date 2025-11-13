namespace Application.Interfaces;

using Contracts;

public interface IBookService
{
    Task<BookResponseModel> GetBookByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<BookResponseModel>> SearchAsync(string query, CancellationToken ct = default);
    Task<IEnumerable<BookResponseModel>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<BookResponseModel>> GetAllAvailableAsync(CancellationToken ct = default);
    Task AddBookAsync(BookUpsertModel model, CancellationToken ct = default);
    Task DeleteBookAsync(Guid id, CancellationToken ct = default);
    Task BorrowBookAsync(Guid id, CancellationToken ct = default);
    Task ReturnBookAsync(Guid id, CancellationToken ct = default);
}