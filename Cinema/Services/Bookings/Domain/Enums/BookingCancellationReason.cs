namespace Domain.Enums;

public enum BookingCancellationReason
{
    UserCancelled = 0,
    PaymentFailed = 1,
    PaymentExpired = 2,
    ShowtimeCancelled = 3,
    AdminCancelled = 4,
}