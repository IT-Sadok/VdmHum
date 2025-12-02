namespace Domain.ValueObjects;

using Enums;

public sealed record Money(decimal Amount, Currency Currency)
{
    public static Money From(decimal amount, Currency currency)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }

        return new Money(amount, currency);
    }
}