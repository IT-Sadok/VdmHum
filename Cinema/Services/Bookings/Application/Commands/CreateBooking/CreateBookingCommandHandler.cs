namespace Application.Commands.CreateBooking;

using Abstractions.Repositories;
using Abstractions.Services;
using Contracts.Bookings;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Errors;
using Microsoft.Extensions.Options;
using Options;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CreateBookingCommandHandler(
    IBookingRepository bookingRepository,
    IMoviesClient moviesClient,
    IPaymentsClient paymentsClient,
    IOptions<BookingOptions> bookingOptions,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateBookingCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken ct)
    {
        var userId = userContextService.GetUserContext().UserId!.Value;

        var showtimeSnapshot = await moviesClient
            .GetShowtimeSnapshotAsync(command.ShowtimeId, ct);

        if (showtimeSnapshot is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
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
        var reservationExpiresAt = now.AddMinutes(bookingOptions.Value.ReservationDurationMinutes);

        var booking = Booking.Create(
            userId: userId,
            showtime: showtimeSnapshot,
            seats: seats,
            totalPrice: totalPrice,
            reservationExpiresAtUtc: reservationExpiresAt);

        bookingRepository.Add(booking);
        await unitOfWork.SaveChangesAsync(ct);

        var paymentResult = await paymentsClient.CreatePaymentForBookingAsync(
            userId: userId,
            bookingId: booking.Id,
            amount: totalPrice.Amount,
            currency: totalPrice.Currency,
            description: $"Booking {booking.Id} for showtime {showtimeSnapshot.ShowtimeId}",
            ct: ct);

        if (paymentResult.IsFailure)
        {
            booking.CancelBySystem(BookingCancellationReason.PaymentNotCreated);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Failure<BookingResponseModel>(PaymentErrors.CreationFailed);
        }

        booking.SetPayment(paymentResult.Value.PaymentId);
        await unitOfWork.SaveChangesAsync(ct);

        return booking.ToResponse(includeTickets: false);
    }
}