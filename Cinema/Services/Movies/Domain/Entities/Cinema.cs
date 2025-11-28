namespace Domain.Entities;

public sealed class Cinema
{
    private readonly HashSet<Hall> _halls = [];

    private Cinema(
        Guid id,
        string name,
        string city,
        string address,
        double? latitude,
        double? longitude)
    {
        this.Id = id;
        this.Name = name;
        this.City = city;
        this.Address = address;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public string City { get; private set; }

    public string Address { get; private set; }

    public double? Latitude { get; private set; }

    public double? Longitude { get; private set; }

    public IReadOnlyCollection<Hall> Halls => this._halls;

    public static Cinema Create(
        string name,
        string city,
        string address,
        double? latitude,
        double? longitude)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required.", nameof(city));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address is required.", nameof(address));
        }

        if ((latitude.HasValue && !longitude.HasValue) ||
            (!latitude.HasValue && longitude.HasValue))
        {
            throw new ArgumentException("You must specify both latitude and longitude, or specify neither");
        }

        var cinema = new Cinema(
            id: Guid.CreateVersion7(),
            name: name,
            city: city,
            address: address,
            latitude: latitude,
            longitude: longitude);

        return cinema;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        this.Name = name;
    }

    public void UpdateCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required.", nameof(city));
        }

        this.City = city;
    }

    public void UpdateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address is required.", nameof(address));
        }

        this.Address = address;
    }

    public void UpdateLatitudeAndLongitude(double latitude, double longitude)
    {
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    internal void AddHall(Hall hall)
    {
        ArgumentNullException.ThrowIfNull(hall);

        if (hall.CinemaId != this.Id)
        {
            throw new InvalidOperationException("Cannot add a hall that belongs to a different cinema.");
        }

        this._halls.Add(hall);
    }
}