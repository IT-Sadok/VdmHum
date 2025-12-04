namespace Application.Commands.CreateBooking;

using Abstractions.Messaging;
using Contracts.Bookings;
using Domain.Enums;

public sealed record CreateBookingCommand(
    Guid ShowtimeId,
    IReadOnlyCollection<int> Seats,
    decimal TotalPrice,
    Currency Currency
) : ICommand<BookingResponseModel>;