namespace Application.Commands.CreateBooking;

using Abstractions;
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
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateBookingCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken ct)
    {
        var userContext = userContextService.Get();

        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure<BookingResponseModel>(CommonErrors.Unauthorized);
        }

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
            userId: userContext.UserId.Value,
            showtime: showtimeSnapshot,
            seats: seats,
            totalPrice: totalPrice,
            reservationExpiresAtUtc: reservationExpiresAt);

        bookingRepository.Add(booking);
        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse(includeTickets: false);
    }
}