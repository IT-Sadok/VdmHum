namespace Application.Queries.GetBookings;

using Abstractions;
using Contracts;
using Contracts.Bookings;

public sealed record GetBookingsQuery(
    PagedQuery<BookingFilter> Filter
) : IQuery<PagedResponse<BookingResponseModel>>;