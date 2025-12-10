namespace Application.Commands.HandleProviderRefundSucceeded;

using Abstractions.Repositories;
using Domain.Enums;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class HandleProviderRefundSucceededCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentRefundRepository refundRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<HandleProviderRefundSucceededCommand>
{
    public async Task<Result> HandleAsync(
        HandleProviderRefundSucceededCommand command,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.ProviderRefundId))
        {
            return Result.Failure(CommonErrors.InvalidId);
        }

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

        var refundedAmount = payment.Refunds
            .Where(r => r.Status == RefundStatus.Succeeded)
            .Sum(r => r.Amount.Amount);

        var remainingAmountToRefund = payment.Amount with
        {
            Amount = payment.Amount.Amount - refundedAmount
        };

        payment.CompleteRefund(refund.Id, remainingAmountToRefund, command.SucceededAtUtc);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}