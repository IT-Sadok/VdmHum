namespace Application.Queries.GetBookings;

using Contracts;
using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record GetBookingsQuery(
    PagedQuery<BookingFilter> Filter
) : IQuery<PagedResponse<BookingResponseModel>>;