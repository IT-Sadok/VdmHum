namespace Application.Commands.CompleteRefund;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Domain.Abstractions;
using Domain.Errors;

public sealed class CompleteRefundCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CompleteRefundCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(
        CompleteRefundCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, ct);

        if (booking is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        booking.CompleteRefund(command.RefundId);

        await unitOfWork.SaveChangesAsync(ct);

        return booking.Id;
    }
}