using Application.Interfaces;

namespace Infrastructure.FileStorage;

using System.Text.Json;

public sealed class JsonFileStorage : IFileStorage
{
    public async Task<T?> ReadAsync<T>(string path, JsonSerializerOptions options, CancellationToken ct = default)
    {
        if (!File.Exists(path))
        {
            return default;
        }

        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream, options, ct);
    }

    public async Task WriteAsync<T>(string path, T data, JsonSerializerOptions options, CancellationToken ct = default)
    {
        await using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, data, options, ct);
    }
}