namespace Presentation.Endpoints.Bookings;

using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Bookings;
using Application.Queries.GetBookings;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetBookingsUser : IEndpoint
{
    public sealed record GetBookingsUserRequest(
        [FromQuery] BookingStatus? Status,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(BookingsRoutes.GetPagedForUser, async (
                [AsParameters] GetBookingsUserRequest request,
                IUserContextService userContextService,
                IQueryHandler<GetBookingsQuery, PagedResponse<BookingResponseModel>> handler,
                CancellationToken ct) =>
            {
                var userContext = userContextService.Get();

                if (!userContext.IsAuthenticated || userContext.UserId is null)
                {
                    return Results.Unauthorized();
                }

                var query = new GetBookingsQuery(
                    new PagedQuery<BookingFilter>(
                        new BookingFilter(
                            UserId: userContext.UserId,
                            Status: request.Status),
                        Page: request.Page,
                        PageSize: request.PageSize));

                var result = await handler.HandleAsync(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Bookings);
    }
}