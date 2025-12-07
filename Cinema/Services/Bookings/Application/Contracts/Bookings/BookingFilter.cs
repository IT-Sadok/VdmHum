namespace Application.Contracts.Bookings;

using Domain.Enums;

public sealed record BookingFilter(
    Guid? UserId,
    BookingStatus? Status)
{
    public BookingFilter(BookingStatus? status)
        : this(UserId: null, Status: status)
    {
    }
}