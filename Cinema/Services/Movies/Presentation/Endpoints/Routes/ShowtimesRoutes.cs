namespace Presentation.Endpoints.Routes;

public static class ShowtimesRoutes
{
    public const string GetPaged = Base;

    public const string GetById = Base + "/{id:guid}";

    public const string Create = Base;

    public const string Update = Base + "/{id:guid}";

    public const string Delete = Base + "/{id:guid}";

    private const string Base = "/api/showtimes";
}