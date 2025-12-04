namespace Domain.Enums;

public enum BookingCancellationReason
{
    UserRefunded = 0,
    PaymentCanceled = 1,
    PaymentFailed = 2,
    PaymentExpired = 3,
    ShowtimeCancelled = 4,
    AdminCancelled = 5,
}