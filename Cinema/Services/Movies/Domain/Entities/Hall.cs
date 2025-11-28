namespace Domain.Entities;

public sealed class Hall
{
    private Hall(
        Guid id,
        Guid cinemaId,
        string name,
        int numberOfSeats)
    {
        this.Id = id;
        this.CinemaId = cinemaId;
        this.Name = name;
        this.NumberOfSeats = numberOfSeats;
    }

    public Guid Id { get; }

    public Guid CinemaId { get; }

    public string Name { get; private set; }

    public int NumberOfSeats { get; private set; }

    public static Hall Create(Guid cinemaId, string name, int numberOfSeats)
    {
        if (cinemaId == Guid.Empty)
        {
            throw new ArgumentException("CinemaId is required.", nameof(cinemaId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (numberOfSeats <= 0)
        {
            throw new ArgumentException("Number of seats is required.", nameof(numberOfSeats));
        }

        return new Hall(
            id: Guid.CreateVersion7(),
            cinemaId: cinemaId,
            name: name.Trim(),
            numberOfSeats: numberOfSeats);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        this.Name = name.Trim();
    }
}