namespace CarFixFiler.Domain.ValueObjects;

/// <summary>
/// Value Object representing a unique Service identifier.
/// Composed of LicensePlateNumber and Date.
/// </summary>
public class ServiceId : IEquatable<ServiceId>
{
    public string LicensePlateNumber { get; }
    public DateTime Date { get; }

    public ServiceId(string licensePlateNumber, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(licensePlateNumber))
            throw new ArgumentException("License plate number cannot be empty", nameof(licensePlateNumber));
        
        LicensePlateNumber = licensePlateNumber;
        Date = date;
    }

    public override bool Equals(object? obj) => Equals(obj as ServiceId);

    public bool Equals(ServiceId? other)
    {
        if (other is null) return false;
        return LicensePlateNumber == other.LicensePlateNumber && Date == other.Date;
    }

    public override int GetHashCode() => HashCode.Combine(LicensePlateNumber, Date);

    public override string ToString() => $"{LicensePlateNumber}_{Date:yyyyMMddHHmmss}";
}
