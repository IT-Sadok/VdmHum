namespace Application.Commands.SystemCancelBooking;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Domain.Abstractions;
using Domain.Errors;

public sealed class SystemCancelBookingCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<SystemCancelBookingCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        SystemCancelBookingCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, ct);

        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        booking.CancelBySystem(command.Reason);

        await unitOfWork.SaveChangesAsync(ct);

        return booking.Id;
    }
}