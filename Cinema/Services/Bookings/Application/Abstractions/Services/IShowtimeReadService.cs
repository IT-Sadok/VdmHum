namespace Application.Abstractions.Services;

using Domain.ValueObjects;

public interface IShowtimeReadService
{
    Task<ShowtimeSnapshot?> GetShowtimeSnapshotAsync(
        Guid showtimeId,
        CancellationToken ct);
}