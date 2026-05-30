namespace CarFixFiler.Domain.ValueObjects;

/// <summary>
/// Value Object representing a unique Customer identifier.
/// </summary>
public class CustomerId : IEquatable<CustomerId>
{
    public string Name { get; }

    public CustomerId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));
        
        Name = name;
    }

    public override bool Equals(object? obj) => Equals(obj as CustomerId);

    public bool Equals(CustomerId? other)
    {
        if (other is null) return false;
        return Name == other.Name;
    }

    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString() => Name;
}
