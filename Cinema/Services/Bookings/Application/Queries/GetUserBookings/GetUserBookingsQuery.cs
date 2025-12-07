namespace Application.Queries.GetUserBookings;

using Abstractions;
using Contracts;
using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record GetUserBookingsQuery(
    PagedFilter<BookingFilter> PagedFilter
) : IQuery<PagedResponse<BookingResponseModel>>, IAuthenticationRequired;