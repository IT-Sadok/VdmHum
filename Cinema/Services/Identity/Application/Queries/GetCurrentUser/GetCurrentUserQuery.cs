namespace Application.Queries.GetCurrentUser;

using Contracts;
using Shared.Contracts.Abstractions;

public sealed record GetCurrentUserQuery : IQuery<UserResponseModel>;