namespace Application.Commands.FailRefund;

using Abstractions.Repositories;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class FailRefundCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<FailRefundCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        FailRefundCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, asNoTracking: false, ct);

        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        booking.FailRefund(command.RefundId, command.FailureReason);

        await unitOfWork.SaveChangesAsync(ct);

        return booking.Id;
    }
}