namespace Application.Queries.GetHall;

using Shared.Contracts.Abstractions;
using Contracts.Halls;

public sealed record GetHallByIdQuery(Guid Id) : IQuery<HallResponseModel>;