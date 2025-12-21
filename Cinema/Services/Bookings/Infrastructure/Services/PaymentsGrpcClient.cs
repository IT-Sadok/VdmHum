namespace Infrastructure.Services;

using System.Globalization;
using Application.Abstractions.Services;
using Payments.Grpc;
using CancelPaymentResponse = Application.Contracts.Payments.CancelPaymentResponse;
using CreatePaymentForBookingResponse = Application.Contracts.Payments.CreatePaymentForBookingResponse;

public class PaymentsGrpcClient(Payments.PaymentsClient client)
    : IPaymentsClient
{
    public async Task<CreatePaymentForBookingResponse> CreatePaymentForBookingAsync(
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