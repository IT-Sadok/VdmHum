namespace Application.Commands.HandleProviderRefundFailer;

using Abstractions.Repositories;
using Domain.Enums;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class HandleProviderRefundFailedCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentRefundRepository refundRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<HandleProviderRefundFailedCommand>
{
    public async Task<Result> HandleAsync(
        HandleProviderRefundFailedCommand command,
        CancellationToken ct)
    {
        var refund = await refundRepository
            .GetByProviderRefundIdAsync(command.ProviderRefundId, asNoTracking: true, ct);

        if (refund is null)
        {
            return Result.Failure(CommonErrors.NotFound);
        }

        var payment = await paymentRepository.GetByIdAsync(refund.PaymentId, asNoTracking: false, ct);

        if (payment is null)
        {
            return Result.Failure(CommonErrors.NotFound);
        }

        if (refund.Status is not RefundStatus.Requested)
        {
            return Result.Failure(PaymentRefundErrors.AlreadyProcessed);
        }

        payment.FailRefund(
            refundId: refund.Id,
            failureCode: command.FailureCode,
            failureMessage: command.FailureMessage,
            failedAtUtc: command.FailedAtUtc);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}