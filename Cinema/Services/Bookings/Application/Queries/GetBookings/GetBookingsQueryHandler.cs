namespace Application.Queries.GetBookings;

using Abstractions;
using Abstractions.Repositories;
using Contracts;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Entities;

public sealed class GetBookingsQueryHandler(
    IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingsQuery, PagedResponse<BookingResponseModel>>
{
    public async Task<Result<PagedResponse<BookingResponseModel>>> HandleAsync(
        GetBookingsQuery query,
        CancellationToken ct)
    {
        var (bookings, totalCount) = await bookingRepository.GetPagedAsync(
            query.Filter,
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