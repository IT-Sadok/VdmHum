namespace Presentation.Endpoints.Payments;

using Application.Contracts.Payments;
using Application.Queries.GetPaymentByIdQuery;
using ErrorHandling;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

public class GetPaymentById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PaymentsRoutes.GetById, async (
                [FromRoute] Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetPaymentByIdQuery(id);

                var result = await mediator.ExecuteQueryAsync<GetPaymentByIdQuery, PaymentResponseModel>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Payments);
    }
}