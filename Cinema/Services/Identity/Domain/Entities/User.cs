namespace Domain.Entities;

public sealed class User
{
    private readonly List<string> _roles = [];

    private User(Guid id, string email, string phoneNumber, string? firstName, string? lastName)
    {
        this.Id = id;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    public Guid Id { get; }

    public string Email { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public IReadOnlyCollection<string> Roles => this._roles.AsReadOnly();

    public static User Create(string email, string phoneNumber, string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty", nameof(email));
        }

        return new User(Guid.CreateVersion7(), email, phoneNumber, firstName, lastName);
    }

    public static User FromIdentity(
        Guid id,
        string email,
        string? phoneNumber,
        string? firstName,
        string? lastName,
        IEnumerable<string>? roles = null)
    {
        var user = new User(id, email, phoneNumber ?? string.Empty, firstName, lastName);

        if (roles is null)
        {
            return user;
        }

        foreach (var role in roles)
        {
            user.AddRole(role);
        }

        return user;
    }

    public void ChangeProfile(
        string? firstName,
        string? lastName,
        string? phone)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.PhoneNumber = phone;
    }

    public void AddRole(string role)
    {
        if (!this._roles.Contains(role))
        {
            this._roles.Add(role);
        }
    }

    public void RemoveRole(string role)
    {
        this._roles.Remove(role);
    }

    public bool HasRole(string role) => this._roles.Contains(role);
}