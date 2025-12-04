namespace Application.Contracts.Bookings;

public sealed record PagedBookingsResponseModel(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<BookingResponseModel> Items);