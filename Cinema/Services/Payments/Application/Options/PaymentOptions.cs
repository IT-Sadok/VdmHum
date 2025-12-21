namespace Application.Options;

using Domain.Enums;

public sealed class PaymentOptions
{
    public PaymentProvider DefaultProvider { get; init; }
}