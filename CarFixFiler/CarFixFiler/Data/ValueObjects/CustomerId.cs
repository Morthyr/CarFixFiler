namespace CarFixFiler.Data.ValueObjects;

/// <summary>
/// Guard readonly struct for Customer technical ID to prevent ID mix-ups
/// </summary>
public readonly struct CustomerId : IEquatable<CustomerId>
{
    public Guid Value { get; }

    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty", nameof(value));
        Value = value;
    }

    public static CustomerId New() => new(Guid.NewGuid());

    public override bool Equals(object? obj) => obj is CustomerId customerId && Equals(customerId);

    public bool Equals(CustomerId other) => Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(CustomerId left, CustomerId right) => left.Equals(right);

    public static bool operator !=(CustomerId left, CustomerId right) => !left.Equals(right);
}
