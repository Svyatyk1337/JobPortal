using JobPortal.Review.Domain.Common;

namespace JobPortal.Review.Domain.ValueObjects;

public class Rating : ValueObject
{
    public double Value { get; private set; }

    private Rating(double value)
    {
        Value = value;
    }

    public static Rating Create(double value)
    {
        if (value < 0 || value > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5", nameof(value));
        }

        return new Rating(Math.Round(value, 1));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString("F1");

    public static implicit operator double(Rating rating) => rating.Value;
}
