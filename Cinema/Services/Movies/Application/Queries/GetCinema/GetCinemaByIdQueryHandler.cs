namespace Application.Queries.GetCinema;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Cinemas;
using Domain.Abstractions;
using Domain.Errors;

public sealed class GetCinemaByIdQueryHandler(
    ICinemaRepository cinemaRepository)
    : IQueryHandler<GetCinemaByIdQuery, CinemaResponseModel>
{
    public async Task<Result<CinemaResponseModel>> HandleAsync(
        GetCinemaByIdQuery query,
        CancellationToken ct)
    {
        var cinema = await cinemaRepository.GetByIdAsync(query.Id, ct);

        if (cinema is null)
        {
            return Result.Failure<CinemaResponseModel>(CinemaErrors.NotFound(query.Id));
        }

        var response = new CinemaResponseModel(
            cinema.Id,
            cinema.Name,
            cinema.City,
            cinema.Address,
            cinema.Latitude,
            cinema.Longitude);

        return response;
    }
}