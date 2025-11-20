namespace Infrastructure.Providers;

using Application.Abstractions.Providers;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}