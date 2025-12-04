namespace Application.Commands.CancelPendingBooking;

using Abstractions;
using Abstractions.Repositories;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Errors;

public sealed class CancelPendingBookingCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelPendingBookingCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        CancelPendingBookingCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, asNoTracking: false, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        booking.CancelPendingPayment();

        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse();
    }
}