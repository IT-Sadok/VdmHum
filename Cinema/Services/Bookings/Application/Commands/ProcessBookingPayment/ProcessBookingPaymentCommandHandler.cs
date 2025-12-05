namespace Application.Commands.ProcessBookingPayment;

using Abstractions;
using Abstractions.Repositories;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Errors;

public sealed class ProcessBookingPaymentCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ProcessBookingPaymentCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        ProcessBookingPaymentCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, asNoTracking: false, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        if (command.PaymentTime <= booking.ReservationExpiresAtUtc)
        {
            booking.ConfirmPayment(command.PaymentId, command.PaymentTime);
        }
        else
        {
            booking.ProcessLatePayment();
        }

        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse();
    }
}