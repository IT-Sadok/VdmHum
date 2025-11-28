namespace Application.Queries.GetHall;

using Abstractions.Messaging;
using Contracts.Halls;

public sealed record GetHallByIdQuery(Guid Id) : IQuery<HallResponseModel>;