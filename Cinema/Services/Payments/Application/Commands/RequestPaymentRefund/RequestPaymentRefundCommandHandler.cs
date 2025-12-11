namespace Application.Commands.RequestPaymentRefund;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.PaymentProvider;
using Contracts.Payments;
using Domain.Enums;
using Domain.ValueObjects;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class RequestPaymentRefundCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentProviderClient paymentProviderClient,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RequestPaymentRefundCommand, PaymentRefundResponseModel>
{
    public async Task<Result<PaymentRefundResponseModel>> HandleAsync(
        RequestPaymentRefundCommand command,
        CancellationToken ct)
    {
        var payment = await paymentRepository.GetByIdAsync(command.PaymentId, false, ct);

        if (payment is null)
        {
            return Result.Failure<PaymentRefundResponseModel>(CommonErrors.NotFound);
        }

        var refundAmount = Money.From(command.Amount, command.Currency);

        var request = new CreateRefundRequest(
            payment.ProviderPaymentId,
            refundAmount.Amount,
            refundAmount.Currency,
            command.Reason);

        var providerRefundId = await paymentProviderClient.CreateRefundAsync(request, ct);

        var refundedAmount = payment.Refunds
            .Where(r => r.Status == RefundStatus.Succeeded)
            .Sum(r => r.Amount.Amount);

        var remainingAmountToRefund = payment.Amount with
        {
            Amount = payment.Amount.Amount - refundedAmount
        };

        var refund = payment.RequestRefund(
            amount: refundAmount,
            providerRefundId: providerRefundId,
            requestedAtUtc: DateTime.UtcNow,
            bookingRefundId: command.BookingRefundId,
            reason: command.Reason);

        await unitOfWork.SaveChangesAsync(ct);

        return refund.ToResponse();
    }
}