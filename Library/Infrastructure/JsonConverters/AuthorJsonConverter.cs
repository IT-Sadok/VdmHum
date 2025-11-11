namespace Infrastructure.JsonConverters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Exceptions.Author;
using Domain.ValueObjects;

public sealed class AuthorJsonConverter : JsonConverter<Author>
{
    public override Author? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var model = JsonSerializer.Deserialize<AuthorSurrogate>(ref reader, options);
        if (model is null)
        {
            return null;
        }

        return model.Type switch
        {
            AuthorType.Known => Author.Known(model.Name ?? "N/A"),
            AuthorType.Pseudonym => Author.Pseudonym(model.Name ?? "N/A"),
            AuthorType.Anonymous => Author.Anonymous(),
            AuthorType.Folk => Author.Folk(),
            AuthorType.Unknown => Author.Unknown(),
            _ => throw new InvalidAuthorTypeException(model.Type.ToString())
        };
    }

    public override void Write(Utf8JsonWriter writer, Author value, JsonSerializerOptions options)
    {
        var model = new AuthorSurrogate
        {
            Name = string.IsNullOrWhiteSpace(value.Name) ? "N/A" : value.Name,
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