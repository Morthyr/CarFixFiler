namespace CarFixFiler.Data.ValueObjects;

/// <summary>
/// Guard readonly struct for Car technical ID to prevent ID mix-ups
/// </summary>
public readonly struct CarId : IEquatable<CarId>
{
    public Guid Value { get; }

    public CarId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CarId cannot be empty", nameof(value));
        Value = value;
    }

    public static CarId New() => new(Guid.NewGuid());

    public override bool Equals(object? obj) => obj is CarId carId && Equals(carId);

    public bool Equals(CarId other) => Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(CarId left, CarId right) => left.Equals(right);

    public static bool operator !=(CarId left, CarId right) => !left.Equals(right);
}
