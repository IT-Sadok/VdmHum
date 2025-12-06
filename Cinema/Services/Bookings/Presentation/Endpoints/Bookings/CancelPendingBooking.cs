namespace Presentation.Endpoints.Bookings;

using Application.Commands.CancelPendingBooking;
using Application.Contracts.Bookings;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class CancelPendingBooking : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BookingsRoutes.Cancel, async (
                Guid id,
                ICommandHandler<CancelPendingBookingCommand, BookingResponseModel> handler,
                CancellationToken ct) =>
            {
                var command = new CancelPendingBookingCommand(BookingId: id);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}