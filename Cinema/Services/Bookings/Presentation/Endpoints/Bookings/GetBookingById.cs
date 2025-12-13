namespace Presentation.Endpoints.Bookings;

using Application.Contracts.Bookings;
using Application.Queries.GetBookingById;
using Extensions;
using ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetBookingById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BookingsRoutes.GetById, async (
                [FromRoute] Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetBookingByIdQuery(id);

                var result = await mediator.ExecuteQueryAsync<GetBookingByIdQuery, BookingResponseModel>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}