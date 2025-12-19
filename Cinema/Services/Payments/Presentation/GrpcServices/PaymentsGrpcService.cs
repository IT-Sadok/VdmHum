namespace Presentation.GrpcServices;

using Application.Commands.CancelPayment;
using Application.Commands.CreatePayment;
using Application.Contracts.Payments;
using Grpc.Core;
using Payments.Grpc;
using Shared.Contracts.Abstractions;
using Currency = Domain.Enums.Currency;

public sealed class PaymentsGrpcService(IMediator mediator) : Payments.PaymentsBase
{
    public override async Task<CreatePaymentForBookingResponse> CreatePaymentForBooking(
        CreatePaymentForBookingRequest request,
        ServerCallContext context)
    {
        var bookingId = Guid.Parse(request.BookingId);

        var command = new CreatePaymentCommand(
            BookingId: bookingId,
            Amount: (decimal)request.Amount,
            Currency: (Currency)request.Currency,
            Description: request.Description);

        var result = await mediator.ExecuteCommandAsync
            <CreatePaymentCommand, PaymentResponseModel>(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.Error!.Description));
        }

        return new CreatePaymentForBookingResponse
        {
            PaymentId = result.Value.Id.ToString(),
        };
    }

    public override async Task<CancelPaymentResponse> CancelPayment(
        CancelPaymentRequest request,
        ServerCallContext context)
    {
        var paymentId = Guid.Parse(request.PaymentId);

        var command = new CancelPaymentCommand(PaymentId: paymentId);

        var result = await mediator.ExecuteCommandAsync
            <CancelPaymentCommand, PaymentResponseModel>(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.Error!.Description));
        }

        return new CancelPaymentResponse
        {
            Canceled = true,
        };
    }
}