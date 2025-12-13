namespace Application.Commands.CancelPendingBooking;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Bookings;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CancelPendingBookingCommandHandler(
    IBookingRepository bookingRepository,
    IPaymentsClient paymentsClient,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelPendingBookingCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        CancelPendingBookingCommand command,
        CancellationToken ct)
    {
        var userId = userContextService.Get().UserId!.Value;

        var booking = await bookingRepository.GetByIdAsync(command.BookingId, asNoTracking: false, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        if (booking.UserId != userId)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.UserIdNotMatch);
        }

        if (booking.PaymentId is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        booking.CancelPendingPayment();

        await unitOfWork.SaveChangesAsync(ct);

        await paymentsClient.CancelPaymentAsync(
            paymentId: booking.PaymentId.Value,
            ct: ct);

        return booking.ToResponse();
    }
}