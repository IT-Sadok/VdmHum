namespace Presentation.Endpoints.Bookings;

using Application.Contracts.Bookings;
using Application.Queries.GetBookingById;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetBookingById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BookingsRoutes.GetById, async (
                Guid id,
                IQueryHandler<GetBookingByIdQuery, BookingResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetBookingByIdQuery(id);

                var result = await handler.HandleAsync(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}