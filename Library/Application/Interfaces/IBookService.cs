using Shared.Contracts;

namespace Application.Interfaces;

public interface IBookService
{
    Task<BookDto> GetBookByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<BookDto>> SearchAsync(string query, CancellationToken ct = default);
    Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<BookDto>> GetAllAvailableAsync(CancellationToken ct = default);
    Task AddBookAsync(BookUpsertDto dto, CancellationToken ct = default);
    Task DeleteBookAsync(Guid id, CancellationToken ct = default);
    Task BorrowBookAsync(Guid id, CancellationToken ct = default);
    Task ReturnBookAsync(Guid id, CancellationToken ct = default);
}