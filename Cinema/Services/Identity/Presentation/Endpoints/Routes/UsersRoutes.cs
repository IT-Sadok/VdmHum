namespace Presentation.Endpoints.Routes;

public static class UsersRoutes
{
    // Auth
    public const string Login = $"{AuthPrefix}login";

    public const string Logout = $"{AuthPrefix}logout";

    public const string Refresh = $"{AuthPrefix}refresh";

    public const string Register = $"{AuthPrefix}register";

    // Users
    public const string GetCurrent = $"{UsersPrefix}me";

    public const string UpdateProfile = $"{UsersPrefix}me";

    // Admins
    public const string CreateAdmin = $"{AdminsPrefix}create";

    private const string Base = "api/";

    private const string AuthPrefix = $"{Base}auth/";

    private const string AdminsPrefix = $"{Base}admins/";

    private const string UsersPrefix = $"{Base}users/";
}