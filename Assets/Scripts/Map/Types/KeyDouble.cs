using Newtonsoft.Json;
using System;

public class KeyDouble : IEquatable<KeyDouble>
{
    [JsonProperty("a")]
    public Key A => _a;
    [JsonProperty("b")]
    public Key B => _b;

    private readonly Key _a, _b;

    [JsonConstructor]
    public KeyDouble(Key a, Key b)
    {
        _a = a; _b = b;
    }

    public bool Equals(KeyDouble other) => other == this;
    public override bool Equals(object obj) => obj as KeyDouble == this;

    public override int GetHashCode() => _a.GetHashCode() ^ _b.GetHashCode();

    //public static bool operator ==(KeyDouble lhs, KeyDouble rhs) => (lhs is null && rhs is null) || (lhs is not null && rhs is not null && lhs.GetHashCode() == rhs.GetHashCode());
    public static bool operator ==(KeyDouble lhs, KeyDouble rhs) => (lhs is null && rhs is null) || (lhs is not null && rhs is not null && ((lhs._a == rhs._a && lhs._b == rhs._b) || (lhs._a == rhs._b && lhs._b == rhs._a)));
    public static bool operator !=(KeyDouble lhs, KeyDouble rhs) => !(lhs == rhs);

    public override string ToString() => $"({_a}, {_b})";
}
