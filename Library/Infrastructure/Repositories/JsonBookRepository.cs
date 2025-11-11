namespace Infrastructure.Repositories;

using Application.Mappers;
using Domain.Entities;
using Shared.Contracts;
using System.Text.Json;
using Domain.Interfaces;

public class JsonBookRepository(string filePath, JsonSerializerOptions jsonOptions) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default)
    {
        if (!File.Exists(filePath))
        {
            return new List<Book>();
        }

        try
        {
            await using var stream = File.OpenRead(filePath);
            var dtos = await JsonSerializer.DeserializeAsync<List<BookDto>>(stream, jsonOptions, ct)
                       ?? [];

            return dtos.Select(BookMapper.FromDto).ToList();
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Invalid JSON structure in '{filePath}'.", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to read file '{filePath}'.", ex);
        }
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

    private async Task SaveAsync(List<Book> books, CancellationToken ct = default)
    {
        var dtoList = books.Select(BookMapper.ToDto).ToList();

        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, dtoList, jsonOptions, ct);
    }
}