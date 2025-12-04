namespace Application.Queries.GetBookingById;

using Abstractions.Messaging;
using Contracts.Bookings;

public sealed record GetBookingByIdQuery(Guid BookingId)
    : IQuery<BookingResponseModel>;