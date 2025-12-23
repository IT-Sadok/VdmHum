namespace Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }

    public DateTime OccurredOnUtc { get; init; }

    public string Type { get; init; } = null!;

    public string Content { get; init; } = null!;

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}