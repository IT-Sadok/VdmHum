namespace Infrastructure.Messaging;

using System.Text.Json;

public sealed class EventJsonOptions
{
    public JsonSerializerOptions Options { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };
}