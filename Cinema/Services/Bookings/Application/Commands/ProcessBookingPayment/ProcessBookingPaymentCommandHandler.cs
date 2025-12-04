namespace Application.Commands.ProcessBookingPayment;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;

public sealed class ProcessBookingPaymentCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ProcessBookingPaymentCommand, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        ProcessBookingPaymentCommand command,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        if (command.PaymentTime <= booking.ReservationExpiresAtUtc)
        {
            booking.ConfirmPayment(command.PaymentId, command.PaymentTime);
        }
        else
        {
            booking.ProcessLatePayment();
        }

        await unitOfWork.SaveChangesAsync(ct);

        var response = MapToResponse(booking);

        return response;
    }

    private static BookingResponseModel MapToResponse(Booking booking)
    {
        var showtime = booking.Showtime;

        var tickets = booking.Tickets
            .Select(t => new TicketResponseModel(
                t.Id,
                t.SeatNumber,
                t.TicketNumber,
                t.Status))
            .ToArray();

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
            Tickets: tickets);
    }
}