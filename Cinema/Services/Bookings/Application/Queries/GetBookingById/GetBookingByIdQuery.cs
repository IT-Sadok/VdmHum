namespace Application.Queries.GetBookingById;

using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record GetBookingByIdQuery(Guid BookingId)
    : IQuery<BookingResponseModel>;