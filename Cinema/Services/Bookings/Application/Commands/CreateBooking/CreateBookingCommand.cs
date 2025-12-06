namespace Application.Commands.CreateBooking;

using Contracts.Bookings;
using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record CreateBookingCommand(
    Guid ShowtimeId,
    IReadOnlyCollection<int> Seats,
    decimal TotalPrice,
    Currency Currency
) : ICommand<BookingResponseModel>;