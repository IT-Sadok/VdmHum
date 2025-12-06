namespace Presentation.Endpoints.Bookings;

using Application.Commands.RequestRefund;
using Application.Contracts.Bookings;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class RequestRefundEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BookingsRoutes.RequestRefund, async (
                Guid id,
                ICommandHandler<RequestRefundCommand, BookingResponseModel> handler,
                CancellationToken ct) =>
            {
                var command = new RequestRefundCommand(BookingId: id);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}