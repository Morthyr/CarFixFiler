namespace CarFixFiler.Domain.ValueObjects;

/// <summary>
/// Value Object representing a unique Car identifier.
/// </summary>
public class CarId : IEquatable<CarId>
{
    public string LicensePlateNumber { get; }

    public CarId(string licensePlateNumber)
    {
        if (string.IsNullOrWhiteSpace(licensePlateNumber))
            throw new ArgumentException("License plate number cannot be empty", nameof(licensePlateNumber));
        
        LicensePlateNumber = licensePlateNumber;
    }

    public override bool Equals(object? obj) => Equals(obj as CarId);

    public bool Equals(CarId? other)
    {
        if (other is null) return false;
        return LicensePlateNumber == other.LicensePlateNumber;
    }

    public override int GetHashCode() => LicensePlateNumber.GetHashCode();

    public override string ToString() => LicensePlateNumber;
}
