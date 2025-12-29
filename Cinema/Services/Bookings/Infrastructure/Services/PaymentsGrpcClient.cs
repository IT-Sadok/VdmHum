namespace Infrastructure.Services;

using System.Globalization;
using Application.Abstractions.Services;
using Application.Errors;
using Grpc.Core;
using Shared.Contracts.Core;
using Payments.Grpc;
using CancelPaymentResponse = Application.Contracts.Payments.CancelPaymentResponse;
using CreatePaymentForBookingResponse = Application.Contracts.Payments.CreatePaymentForBookingResponse;

public class PaymentsGrpcClient(Payments.PaymentsClient client)
    : IPaymentsClient
{
    public async Task<Result<CreatePaymentForBookingResponse>> CreatePaymentForBookingAsync(
        Guid userId,
        Guid bookingId,
        decimal amount,
        Domain.Enums.Currency currency,
        string description,
        CancellationToken ct)
    {
        var request = new CreatePaymentForBookingRequest
        {
            UserId = userId.ToString(),
            BookingId = bookingId.ToString(),
            Amount = Convert.ToDouble(amount, CultureInfo.InvariantCulture),
            Currency = (Currency)currency,
            Description = description,
        };

        try
        {
            var response = await client.CreatePaymentForBookingAsync(request, cancellationToken: ct);

            var dto = new CreatePaymentForBookingResponse(Guid.Parse(response.PaymentId));

            return Result.Success(dto);
        }
        catch (RpcException e)
        {
            return Result.Failure<CreatePaymentForBookingResponse>(PaymentErrors.Unexpected(e));
        }
    }

    public async Task<Result<CancelPaymentResponse>> CancelPaymentAsync(Guid paymentId, CancellationToken ct)
    {
        var request = new CancelPaymentRequest
        {
            PaymentId = paymentId.ToString(),
        };

        try
        {
            var response = await client.CancelPaymentAsync(request, cancellationToken: ct);

            var dto = new CancelPaymentResponse(response.Canceled);

            return Result.Success(dto);
        }
        catch (RpcException e)
        {
            return Result.Failure<CancelPaymentResponse>(PaymentErrors.Unexpected(e));
        }
    }
}