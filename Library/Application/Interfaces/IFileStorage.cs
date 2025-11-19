namespace Application.Interfaces;

using System.Text.Json;

public interface IFileStorage
{
    Task<T?> ReadAsync<T>(string path, JsonSerializerOptions options, CancellationToken ct = default);
    Task WriteAsync<T>(string path, T data, JsonSerializerOptions options, CancellationToken ct = default);
}