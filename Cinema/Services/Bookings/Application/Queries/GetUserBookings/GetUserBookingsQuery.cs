namespace Application.Queries.GetUserBookings;

using Abstractions;
using Contracts;
using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record GetUserBookingsQuery(
    PagedQuery<BookingFilter> Filter
) : IQuery<PagedResponse<BookingResponseModel>>, IAuthenticationRequired;