namespace Application.Commands.RequestPaymentRefund;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Payments;
using Domain.Enums;
using Domain.ValueObjects;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class RequestPaymentRefundCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentProviderClient paymentProviderClient,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RequestPaymentRefundCommand, PaymentRefundResponseModel>
{
    public async Task<Result<PaymentRefundResponseModel>> HandleAsync(
        RequestPaymentRefundCommand command,
        CancellationToken ct)
    {
        if (command.PaymentId == Guid.Empty)
        {
            return Result.Failure<PaymentRefundResponseModel>(CommonErrors.InvalidId);
        }

        if (command.Amount <= 0)
        {
            return Result.Failure<PaymentRefundResponseModel>(CommonErrors.InvalidAmount);
        }

        var payment = await paymentRepository.GetByIdAsync(command.PaymentId, false, ct);

        if (payment is null)
        {
            return Result.Failure<PaymentRefundResponseModel>(CommonErrors.NotFound);
        }

        var refundAmount = Money.From(command.Amount, command.Currency);

        var providerRefundId = await paymentProviderClient.CreateRefundAsync(
            providerPaymentId: payment.ProviderPaymentId,
            amount: refundAmount.Amount,
            currency: refundAmount.Currency,
            reason: command.Reason,
            ct: ct);

        var refundedAmount = payment.Refunds
            .Where(r => r.Status == RefundStatus.Succeeded)
            .Sum(r => r.Amount.Amount);

        var remainingAmountToRefund = payment.Amount with
        {
            Amount = payment.Amount.Amount - refundedAmount
        };

        var refund = payment.RequestRefund(
            amount: refundAmount,
            remainingAmountToRefund: remainingAmountToRefund,
            providerRefundId: providerRefundId,
            requestedAtUtc: dateTimeProvider.UtcNow,
            bookingRefundId: command.BookingRefundId,
            reason: command.Reason);

        await unitOfWork.SaveChangesAsync(ct);

        return refund.ToResponse();
    }
}