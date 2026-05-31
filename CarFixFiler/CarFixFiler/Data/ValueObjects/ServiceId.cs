namespace CarFixFiler.Data.ValueObjects;

/// <summary>
/// Guard readonly struct for Service technical ID to prevent ID mix-ups
/// </summary>
public readonly struct ServiceId : IEquatable<ServiceId>
{
    public Guid Value { get; }

    public ServiceId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ServiceId cannot be empty", nameof(value));
        Value = value;
    }

    public static ServiceId New() => new(Guid.NewGuid());

    public override bool Equals(object? obj) => obj is ServiceId serviceId && Equals(serviceId);

    public bool Equals(ServiceId other) => Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(ServiceId left, ServiceId right) => left.Equals(right);

    public static bool operator !=(ServiceId left, ServiceId right) => !left.Equals(right);
}
