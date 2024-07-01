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

    public bool Equals(KeyDouble other) => other is not null && ((_a == other._a && _b == other._b) || (_a == other._b && _b == other._a));
    public override bool Equals(object obj) => Equals(obj as KeyDouble);

    public override int GetHashCode() => _a.GetHashCode() ^ _b.GetHashCode();

    public static bool operator ==(KeyDouble lhs, KeyDouble rhs) => (lhs is null && rhs is null) || (lhs is not null && lhs.Equals(rhs));
    public static bool operator !=(KeyDouble lhs, KeyDouble rhs) => !(lhs == rhs);

    public override string ToString() => $"({_a}, {_b})";
}
