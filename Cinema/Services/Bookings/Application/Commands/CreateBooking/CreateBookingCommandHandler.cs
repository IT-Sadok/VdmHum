namespace Application.Commands.CreateBooking;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Options;

public sealed class CreateBookingCommandHandler(
    IBookingRepository bookingRepository,
    IShowtimeReadService showtimeReadService,
    IOptions<BookingOptions> bookingOptions,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateBookingCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken ct)
    {
        var showtimeSnapshot = await showtimeReadService
            .GetShowtimeSnapshotAsync(command.ShowtimeId, ct);

        if (showtimeSnapshot is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.ShowtimeNotFound);
        }

        var seats = command.Seats.Distinct().ToArray();

        var seatsAvailable = await bookingRepository
            .AreSeatsAvailableAsync(showtimeSnapshot.ShowtimeId, seats, ct);

        if (!seatsAvailable)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.SeatsUnavailable);
        }

        var totalPrice = Money.From(command.TotalPrice, command.Currency);

        var now = DateTime.UtcNow;
        var reservationExpiresAt = now.AddMinutes(bookingOptions.Value.ReservationDuration);

        var booking = Booking.Create(
            userId: command.UserId,
            showtime: showtimeSnapshot,
            seats: seats,
            totalPrice: totalPrice,
            reservationExpiresAtUtc: reservationExpiresAt);

        bookingRepository.Add(booking);
        await unitOfWork.SaveChangesAsync(ct);

        var response = MapToResponse(booking);

        return response;
    }

    private static BookingResponseModel MapToResponse(Booking booking)
    {
        var showtime = booking.Showtime;

        var emptyTickets = Array.Empty<TicketResponseModel>();

        return new BookingResponseModel(
            Id: booking.Id,
            UserId: booking.UserId,
            ShowtimeId: showtime.ShowtimeId,
            MovieTitle: showtime.MovieTitle,
            CinemaName: showtime.CinemaName,
            HallName: showtime.HallName,
            ShowtimeStartTimeUtc: showtime.StartTimeUtc,
            Status: booking.Status,
            TotalPrice: booking.TotalPrice.Amount,
            Currency: booking.TotalPrice.Currency,
            CreatedAtUtc: booking.CreatedAtUtc,
            ReservationExpiresAtUtc: booking.ReservationExpiresAtUtc,
            Seats: booking.Seats.ToArray(),
            Tickets: emptyTickets);
    }
}