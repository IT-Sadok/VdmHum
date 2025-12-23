namespace Application.Commands.HandleProviderPaymentFailed;

using Abstractions.Repositories;
using Abstractions.Services;
using Domain.Enums;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;
using Shared.Contracts.Events;

public sealed class HandleProviderPaymentFailedCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    IEventPublisher eventPublisher)
    : ICommandHandler<HandleProviderPaymentFailedCommand>
{
    public async Task<Result> HandleAsync(
        HandleProviderPaymentFailedCommand command,
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

        payment.MarkFailed(
            failureCode: command.FailureCode,
            failureMessage: command.FailureMessage,
            failedAtUtc: command.FailedAtUtc);

        var @event = new PaymentTransactionFailEvent(
            PaymentId: payment.Id,
            BookingId: payment.BookingId,
            UserId: payment.UserId);

        await eventPublisher.PublishAsync(@event, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}