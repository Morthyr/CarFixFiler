namespace CarFixFiler.Data.ValueObjects;

/// <summary>
/// Guard readonly struct for ServiceItem technical ID to prevent ID mix-ups
/// </summary>
public readonly struct ServiceItemId : IEquatable<ServiceItemId>
{
    public Guid Value { get; }

    public ServiceItemId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ServiceItemId cannot be empty", nameof(value));
        Value = value;
    }

    public static ServiceItemId New() => new(Guid.NewGuid());

    public override bool Equals(object? obj) => obj is ServiceItemId serviceItemId && Equals(serviceItemId);

    public bool Equals(ServiceItemId other) => Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(ServiceItemId left, ServiceItemId right) => left.Equals(right);

    public static bool operator !=(ServiceItemId left, ServiceItemId right) => !left.Equals(right);
}
