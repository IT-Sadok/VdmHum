namespace Application.Contracts.Bookings;

using Domain.Enums;

public sealed record TicketResponseModel(
    Guid Id,
    int SeatNumber,
    string TicketNumber,
    TicketStatus Status);