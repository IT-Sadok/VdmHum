namespace Application.Queries.GetUserBookings;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts;
using Contracts.Bookings;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetUserBookingsQueryHandler(
    IBookingRepository bookingRepository,
    IUserContextService userContextService)
    : IQueryHandler<GetUserBookingsQuery, PagedResponse<BookingResponseModel>>
{
    public async Task<Result<PagedResponse<BookingResponseModel>>> HandleAsync(
        GetUserBookingsQuery query,
        CancellationToken ct)
    {
        var userId = userContextService.Get().UserId!.Value;

        var filter = query.Filter.Filter with { UserId = userId };

        var (bookings, totalCount) = await bookingRepository.GetPagedAsync(
            query.Filter with
            {
                Filter = filter
            },
            ct);

        var responseItems = bookings
            .Select(b => b.ToResponse(includeTickets: true))
            .ToArray();

        var response = new PagedResponse<BookingResponseModel>(
            Page: query.Filter.Page,
            PageSize: query.Filter.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }
}