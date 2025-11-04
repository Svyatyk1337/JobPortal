using JobPortal.Review.Domain.Common;

namespace JobPortal.Review.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be empty", nameof(currency));
        }

        return new Money(amount, currency.ToUpper());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:N2} {Currency}";
}
