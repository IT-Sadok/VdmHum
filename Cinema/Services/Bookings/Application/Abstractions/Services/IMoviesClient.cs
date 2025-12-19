namespace Application.Abstractions.Services;

using Domain.ValueObjects;

public interface IMoviesClient
{
    Task<ShowtimeSnapshot?> GetShowtimeSnapshotAsync(
        Guid showtimeId,
        CancellationToken ct);
}