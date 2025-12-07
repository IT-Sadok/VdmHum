namespace Presentation.Endpoints.Bookings;

using Application.Contracts;
using Application.Contracts.Bookings;
using Application.Queries.GetUserBookings;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetUserBookings : IEndpoint
{
    public sealed record GetUserBookingsRequest(
        [FromQuery] BookingStatus? Status,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BookingsRoutes.GetPagedForUser, async (
                [AsParameters] GetUserBookingsRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetUserBookingsQuery(
                    new PagedFilter<BookingFilter>(
                        ModelFilter: new BookingFilter(request.Status),
                        Page: request.Page,
                        PageSize: request.PageSize));

                var result = await mediator.ExecuteQueryAsync
                    <GetUserBookingsQuery, PagedResponse<BookingResponseModel>>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}