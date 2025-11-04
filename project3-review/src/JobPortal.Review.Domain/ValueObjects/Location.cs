using JobPortal.Review.Domain.Common;

namespace JobPortal.Review.Domain.ValueObjects;

public class Location : ValueObject
{
    public string City { get; private set; }
    public string Country { get; private set; }

    private Location(string city, string country)
    {
        City = city;
        Country = country;
    }

    public static Location Create(string city, string country)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City cannot be empty", nameof(city));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be empty", nameof(country));
        }

        return new Location(city.Trim(), country.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Country;
    }

    public override string ToString() => $"{City}, {Country}";
}
