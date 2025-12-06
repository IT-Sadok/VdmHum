namespace Application.Commands.RequestRefund;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Bookings;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class RequestRefundCommandHandler(
    IBookingRepository bookingRepository,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RequestRefundCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        RequestRefundCommand command,
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

        booking.RequestRefund();

        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse();
    }
}