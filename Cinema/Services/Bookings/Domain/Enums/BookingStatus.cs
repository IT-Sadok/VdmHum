namespace Domain.Enums;

public enum BookingStatus
{
    PendingPayment = 0,
    Confirmed = 1,
    Cancelled = 2,
    Expired = 3,
    RefundPending = 4,
    Refunded = 5,
}