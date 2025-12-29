namespace Application.Options;

public sealed class ServiceBusOptions
{
    public string ConnectionString { get; init; } = null!;

    public string PublisherTopic { get; init; } = null!;

    public Dictionary<string, string> Topics { get; init; } = new();

    public Dictionary<string, string> Subscriptions { get; init; } = new();
}