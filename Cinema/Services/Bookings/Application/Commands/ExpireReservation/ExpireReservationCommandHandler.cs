namespace Application.Commands.ExpireReservation;

using Abstractions.Repositories;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class ExpireReservationsCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ExpireReservationCommand>
{
    public async Task<Result> HandleAsync(
        ExpireReservationCommand command,
        CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var bookings = await bookingRepository.GetExpiredReservationsAsync(now, ct);

        foreach (var booking in bookings)
        {
            booking.ExpireReservation();
        }

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}