namespace Application.Commands.CancelPendingBooking;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Bookings;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CancelPendingBookingCommandHandler(
    IBookingRepository bookingRepository,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelPendingBookingCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        CancelPendingBookingCommand command,
        CancellationToken ct)
    {
        var userContext = userContextService.Get();

        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure<BookingResponseModel>(CommonErrors.Unauthorized);
        }

        var booking = await bookingRepository.GetByIdAsync(command.BookingId, asNoTracking: false, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        if (booking.UserId != userContext.UserId)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.UserIdNotMatch);
        }

        booking.CancelPendingPayment();

        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse();
    }
}