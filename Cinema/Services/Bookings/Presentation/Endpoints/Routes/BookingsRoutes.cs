namespace Presentation.Endpoints.Routes;

public static class BookingsRoutes
{
    public const string Create = Base;

    public const string GetById = Base + "/{id:guid}";

    public const string GetPagedForUser = Base + "/user";

    public const string GetPaged = Base;

    public const string Cancel = Base + "/{id:guid}/cancel";

    public const string RequestRefund = Base + "/{id:guid}/refund";

    private const string Base = "/api/bookings";
}