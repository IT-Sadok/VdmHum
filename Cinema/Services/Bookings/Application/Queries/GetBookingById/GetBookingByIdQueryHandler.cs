namespace Application.Queries.GetBookingById;

using Abstractions;
using Abstractions.Repositories;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Errors;

public sealed class GetBookingByIdQueryHandler(
    IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingByIdQuery, BookingResponseModel>
{
    public async Task<Result<BookingResponseModel>> HandleAsync(
        GetBookingByIdQuery query,
        CancellationToken ct)
    {
        var booking = await bookingRepository.GetByIdAsync(query.BookingId, asNoTracking: true, ct);

        if (booking is null)
        {
            return Result.Failure<BookingResponseModel>(BookingErrors.NotFound);
        }

        return booking.ToResponse();
    }
}