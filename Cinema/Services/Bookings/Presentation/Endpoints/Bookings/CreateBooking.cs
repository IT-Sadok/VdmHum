namespace Presentation.Endpoints.Bookings;

using Application.Commands.CreateBooking;
using Application.Contracts.Bookings;
using Domain.Enums;
using Extensions;
using ErrorHandling;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class CreateBooking : IEndpoint
{
    public sealed record CreateBookingRequest(
        Guid ShowtimeId,
        IReadOnlyCollection<int> Seats,
        decimal TotalPrice,
        Currency Currency);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(BookingsRoutes.Create, async (
                CreateBookingRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new CreateBookingCommand(
                    ShowtimeId: request.ShowtimeId,
                    Seats: request.Seats,
                    TotalPrice: request.TotalPrice,
                    Currency: request.Currency);

                var result = await mediator.ExecuteCommandAsync
                    <CreateBookingCommand, BookingResponseModel>(command, ct);

                return result.Match(
                    booking => Results.Created(
                        BookingsRoutes.GetById.Replace("{id:guid}", booking.Id.ToString()),
                        booking),
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}