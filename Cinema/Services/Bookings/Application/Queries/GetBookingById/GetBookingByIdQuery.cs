namespace Application.Queries.GetBookingById;

using Abstractions;
using Contracts.Bookings;

public sealed record GetBookingByIdQuery(Guid BookingId)
    : IQuery<BookingResponseModel>;