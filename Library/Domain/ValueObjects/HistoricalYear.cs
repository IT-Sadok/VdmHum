namespace Domain.ValueObjects;

using Exceptions.HistoricalYear;

public readonly struct HistoricalYear : IComparable<HistoricalYear>
{
    public const int MinYearValue = 1;
    public const int MaxYearValue = 9999;

    public int Value { get; }

    public bool IsBC { get; }

    private HistoricalYear(int value, bool isBC)
    {
        if (!IsValid(value))
        {
            throw new InvalidYearException(value);
        }

        this.Value = value;
        this.IsBC = isBC;
    }

    public static HistoricalYear BC(int value) => new HistoricalYear(value, true);
    public static HistoricalYear AD(int value) => new HistoricalYear(value, false);

    public override string ToString() => this.IsBC ? $"{this.Value} BC" : $"{this.Value} AD";

    public int CompareTo(HistoricalYear other)
    {
        return this.IsBC switch
        {
            true when !other.IsBC => -1,
            false when other.IsBC => 1,
            true when other.IsBC => other.Value.CompareTo(this.Value),
            _ => this.Value.CompareTo(other.Value)
        };
    }

    public static bool IsValid(int value) =>
        value is >= MinYearValue and <= MaxYearValue;
}