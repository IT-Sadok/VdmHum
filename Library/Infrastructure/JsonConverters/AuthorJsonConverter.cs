namespace Infrastructure.JsonConverters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.ValueObjects;

public sealed class AuthorJsonConverter : JsonConverter<Author>
{
    public override Author Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var model = JsonSerializer.Deserialize<AuthorSurrogate>(ref reader, options)
                    ?? throw new JsonException("Unable to deserialize Author.");

        return model.Type switch
        {
            AuthorType.Known => Author.Known(model.Name!),
            AuthorType.Pseudonym => Author.Pseudonym(model.Name!),
            AuthorType.Anonymous => Author.Anonymous(),
            AuthorType.Folk => Author.Folk(),
            AuthorType.Unknown => Author.Unknown(),
            _ => throw new JsonException($"Invalid author type: {model.Type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Author value, JsonSerializerOptions options)
    {
        var model = new AuthorSurrogate
        {
            Name = value.Name,
            Type = value.Type
        };

        JsonSerializer.Serialize(writer, model, options);
    }

    private sealed class AuthorSurrogate
    {
        public string? Name { get; init; }
        public AuthorType Type { get; init; }
    }
}