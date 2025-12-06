namespace Application.Queries.GetHall;

using Abstractions.Repositories;
using Contracts.Halls;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetHallByIdQueryHandler(
    IHallRepository hallRepository)
    : IQueryHandler<GetHallByIdQuery, HallResponseModel>
{
    public async Task<Result<HallResponseModel>> HandleAsync(
        GetHallByIdQuery query,
        CancellationToken ct)
    {
        var hall = await hallRepository.GetByIdAsync(query.Id, true, ct);

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