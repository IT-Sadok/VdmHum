namespace Application.Queries.GetHall;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Halls;
using Domain.Abstractions;
using Domain.Errors;

public sealed class GetHallByIdQueryHandler(
    IHallRepository hallRepository)
    : IQueryHandler<GetHallByIdQuery, HallResponseModel>
{
    public async Task<Result<HallResponseModel>> HandleAsync(
        GetHallByIdQuery query,
        CancellationToken ct)
    {
        var hall = await hallRepository.GetByIdAsync(query.Id, ct);

        if (hall is null)
        {
            return Result.Failure<HallResponseModel>(HallErrors.NotFound(query.Id));
        }

        var response = new HallResponseModel(
            hall.Id,
            hall.CinemaId,
            hall.Name,
            hall.NumberOfSeats);

        return response;
    }
}