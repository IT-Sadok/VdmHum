namespace Infrastructure.Services;

using System.Globalization;
using Application.Abstractions.Services;
using Domain.Enums;
using Payments.Grpc;
using CancelPaymentResponse = Application.Contracts.Payments.CancelPaymentResponse;
using CreatePaymentForBookingResponse = Application.Contracts.Payments.CreatePaymentForBookingResponse;

public class PaymentsGrpcClient(Payments.PaymentsClient client)
    : IPaymentsGrpcClient
{
    public async Task<CreatePaymentForBookingResponse> CreatePaymentForBookingAsync(
        Guid bookingId,
        decimal amount,
        Currency currency,
        string description,
        CancellationToken ct)
    {
        var request = new CreatePaymentForBookingRequest
        {
            BookingId = bookingId.ToString(),
            Amount = Convert.ToDouble(amount, CultureInfo.InvariantCulture),
            Currency = (int)currency,
            Description = description,
        };

        var response = await client.CreatePaymentForBookingAsync(request, cancellationToken: ct);

        return new CreatePaymentForBookingResponse(Guid.Parse(response.PaymentId));
    }

    public async Task<CancelPaymentResponse> CancelPaymentAsync(Guid paymentId, CancellationToken ct)
    {
        var request = new CancelPaymentRequest
        {
            PaymentId = paymentId.ToString(),
        };

        var response = await client.CancelPaymentAsync(request, cancellationToken: ct);

        return new CancelPaymentResponse(response.Canceled);
    }
}