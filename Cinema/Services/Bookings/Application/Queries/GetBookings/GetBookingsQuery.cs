namespace Application.Queries.GetBookings;

using Abstractions.Messaging;
using Contracts.Bookings;
using Domain.Enums;

public sealed record GetBookingsQuery(
    Guid? UserId,
    BookingStatus Status,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedBookingsResponseModel>;