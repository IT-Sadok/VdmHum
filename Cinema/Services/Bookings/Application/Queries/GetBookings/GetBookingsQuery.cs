namespace Application.Queries.GetBookings;

using Contracts;
using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record GetBookingsQuery(
    PagedFilter<BookingFilter> PagedFilter
) : IQuery<PagedResponse<BookingResponseModel>>;