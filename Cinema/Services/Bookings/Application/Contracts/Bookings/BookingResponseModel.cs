namespace Application.Contracts.Bookings;

using Domain.Enums;

public sealed record BookingResponseModel(
    Guid Id,
    Guid UserId,
    Guid ShowtimeId,
    string MovieTitle,
    string CinemaName,
    string HallName,
    DateTime ShowtimeStartTimeUtc,
    BookingStatus Status,
    decimal TotalPrice,
    Currency Currency,
    DateTime CreatedAtUtc,
    DateTime ReservationExpiresAtUtc,
    IReadOnlyCollection<int> Seats,
    IReadOnlyCollection<TicketResponseModel> Tickets);