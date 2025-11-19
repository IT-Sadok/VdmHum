namespace Infrastructure.Repositories;

using Contracts;
using Mappers;
using Application.Interfaces;
using Domain.Entities;
using System.Text.Json;

public sealed class JsonBookRepository(
    string filePath,
    IFileStorage storage,
    JsonSerializerOptions jsonOptions)
    : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default)
    {
        var persisted = await storage.ReadAsync<List<BookStorageModel>>(filePath, jsonOptions, ct) ?? [];

        return persisted.Select(BookStorageMapper.FromStorageModel).ToList();
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var all = await GetAllAsync(ct);
        return all.SingleOrDefault(b => b.Id == id);
    }

    public async Task AddAsync(Book book, CancellationToken ct = default)
    {
        var books = (await GetAllAsync(ct)).ToList();
        books.Add(book);
        await SaveAsync(books, ct);
    }

    public async Task UpdateAsync(Book book, CancellationToken ct = default)
    {
        var books = (await GetAllAsync(ct))
            .Select(b => b.Id == book.Id ? book : b)
            .ToList();

        await SaveAsync(books, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var books = (await GetAllAsync(ct)).ToList();
        books.RemoveAll(b => b.Id == id);
        await SaveAsync(books, ct);
    }

    private async Task SaveAsync(List<Book> books, CancellationToken ct)
    {
        var persistenceModels = books.Select(BookStorageMapper.ToStorageModel).ToList();
        await storage.WriteAsync(filePath, persistenceModels, jsonOptions, ct);
    }
}