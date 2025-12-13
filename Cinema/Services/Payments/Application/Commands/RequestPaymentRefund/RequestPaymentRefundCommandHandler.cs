namespace Application.Commands.RequestPaymentRefund;

using System.Net;
using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.PaymentProvider;
using Contracts.Payments;
using Domain.ValueObjects;
using Errors;
using Exceptions;
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

        string providerRefundId;

        try
        {
            providerRefundId = await paymentProviderClient.CreateRefundAsync(request, ct);
        }
        catch (PaymentProviderException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<PaymentRefundResponseModel>(PaymentProviderErrors.BadRequest);
        }
        catch (PaymentProviderException)
        {
            return Result.Failure<PaymentRefundResponseModel>(PaymentProviderErrors.ServerError);
        }

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