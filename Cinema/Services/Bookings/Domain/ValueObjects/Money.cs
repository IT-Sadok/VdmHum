namespace Domain.ValueObjects;

public sealed record Money(decimal Amount, string Currency)
{
    public static Money From(decimal amount, string currency)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency is required.", nameof(currency));
        }

        if (currency.Length != 3)
        {
            throw new ArgumentException("Currency must have exactly three characters.", nameof(currency));
        }

        return new Money(amount, currency.ToUpperInvariant());
    }
}