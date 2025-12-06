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

        booking.RequestRefund();

        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse();
    }
}