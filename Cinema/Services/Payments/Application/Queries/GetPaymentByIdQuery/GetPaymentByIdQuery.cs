namespace Application.Queries.GetPaymentByIdQuery;

using Contracts.Payments;
using Shared.Contracts.Abstractions;

public sealed record GetPaymentByIdQuery(Guid PaymentId)
    : IQuery<PaymentResponseModel>;