namespace Application.Contracts.Bookings;

using Domain.Enums;

public sealed record BookingFilter(
    Guid? UserId,
    BookingStatus? Status);