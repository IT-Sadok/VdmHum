namespace Presentation.Grpc;

using Application.Commands.ProcessBookingPayment;
using Application.Contracts.Bookings;
using Bookings.Grpc;
using global::Grpc.Core;
using Shared.Contracts.Abstractions;

public sealed class BookingsGrpcService(IMediator mediator) : Bookings.BookingsBase
{
    public override async Task<ProcessBookingPaymentResponse> ProcessBookingPayment(
        ProcessBookingPaymentRequest request,
        ServerCallContext context)
    {
        var bookingId = Guid.Parse(request.BookingId);
        var paymentId = Guid.Parse(request.PaymentId);
        var paymentTime = request.PaymentTime.ToDateTime();

        var command = new ProcessBookingPaymentCommand(
            BookingId: bookingId,
            PaymentId: paymentId,
            PaymentTime: paymentTime);

        var result = await mediator.ExecuteCommandAsync
            <ProcessBookingPaymentCommand, BookingResponseModel>(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(StatusCode.NotFound, result.Error!.Description));
        }

        return new ProcessBookingPaymentResponse
        {
            BookingId = request.BookingId,
            Status = (BookingStatus)result.Value.Status,
        };
    }
}