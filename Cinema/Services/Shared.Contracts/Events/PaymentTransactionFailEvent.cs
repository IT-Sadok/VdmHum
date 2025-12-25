namespace Shared.Contracts.Events;

using Abstractions;

public sealed record PaymentTransactionFailEvent(
    Guid PaymentId,
    Guid BookingId,
    Guid UserId
) : Event(EventTypes.PaymentTransactionFailed);