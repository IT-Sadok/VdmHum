namespace Application.Queries.GetCurrentUser;

using Abstractions.Messaging;
using Contracts;

public sealed record GetCurrentUserQuery : IQuery<UserResponseModel>;