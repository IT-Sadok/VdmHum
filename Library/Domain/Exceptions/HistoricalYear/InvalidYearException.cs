namespace Domain.Exceptions.HistoricalYear;

public sealed class InvalidYearException(int value)
    : DomainException($"Year '{value}' is out of allowed range or invalid.")
{
    public int Value { get; } = value;
}