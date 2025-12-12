namespace Application.Abstractions.Services;

using Domain.ValueObjects;

public interface IMoviesGrpcClient
{
    Task<ShowtimeSnapshot?> GetShowtimeSnapshotAsync(
        Guid showtimeId,
        CancellationToken ct);
}