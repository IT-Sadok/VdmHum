namespace Application.Commands.HandleProviderPaymentSucceeded;

using Abstractions.Repositories;
using Abstractions.Services;
using Domain.Enums;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class HandleProviderPaymentSucceededCommandHandler(
    IPaymentRepository paymentRepository,
    IBookingsClient bookingsClient,
    IUnitOfWork unitOfWork)
    : ICommandHandler<HandleProviderPaymentSucceededCommand>
{
    public async Task<Result> HandleAsync(
        HandleProviderPaymentSucceededCommand command,
        CancellationToken ct)
    {
        var payment = await paymentRepository
            .GetByProviderPaymentIdAsync(command.ProviderPaymentId, asNoTracking: false, ct);

        if (payment is null)
        {
            return Result.Failure(CommonErrors.NotFound);
        }

        if (payment.Status is not PaymentStatus.Pending)
        {
            return Result.Failure(PaymentErrors.AlreadyProcessed);
        }

        payment.MarkSucceeded(command.SucceededAtUtc);

        await unitOfWork.SaveChangesAsync(ct);

        await bookingsClient.ProcessBookingPaymentAsync(
            bookingId: payment.BookingId,
            paymentId: payment.Id,
            paymentTime: command.SucceededAtUtc,
            ct: ct);

        return Result.Success();
    }
}