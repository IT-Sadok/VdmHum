namespace Presentation.Endpoints.Bookings;

using Application.Contracts;
using Application.Contracts.Bookings;
using Application.Queries.GetBookings;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetBookingsAdmin : IEndpoint
{
    public sealed record GetBookingsAdminRequest(
        [FromQuery] Guid? UserId,
        [FromQuery] BookingStatus? Status,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BookingsRoutes.GetPaged, async (
                [AsParameters] GetBookingsAdminRequest adminRequest,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetBookingsQuery(
                    new PagedQuery<BookingFilter>(
                        new BookingFilter(
                            UserId: adminRequest.UserId,
                            Status: adminRequest.Status),
                        Page: adminRequest.Page,
                        PageSize: adminRequest.PageSize));

                var result = await mediator.Send(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Bookings);
    }
}