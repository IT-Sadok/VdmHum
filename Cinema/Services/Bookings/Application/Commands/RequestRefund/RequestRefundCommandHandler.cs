namespace Application.Commands.RequestRefund;

using Abstractions.Repositories;
using Contracts.Bookings;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class RequestRefundCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RequestRefundCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        RequestRefundCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, asNoTracking: false, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        booking.RequestRefund();

        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse();
    }
}