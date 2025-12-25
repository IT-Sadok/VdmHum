namespace Application.Options;

public sealed class ExpireReservationsOptions
{
    public int DelaySeconds { get; init; } = 15;
}