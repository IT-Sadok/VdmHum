namespace Application.Options;

public sealed class OutboxProcessorOptions
{
    public int DelaySeconds { get; init; } = 1;

    public int BatchSize { get; init; } = 100;
}