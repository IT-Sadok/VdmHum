namespace Presentation.Endpoints.Routes;

public static class PaymentsRoutes
{
    public const string GetById = Base + "/{id:guid}";

    private const string Base = "/api/payments";
}