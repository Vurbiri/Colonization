using Newtonsoft.Json;
using System;
using UnityEngine;

public class Key : IEquatable<Key>
{
    [JsonProperty("x")]
    public int X => _x;
    [JsonProperty("y")]
    public int Y => _y;
    [JsonIgnore]
    public static Key Zero => _zero;

    private readonly int _x, _y;
    private static readonly Key _zero = new(0, 0);

    [JsonConstructor]
    public Key(int x, int y)
    {
        _x = x; _y = y;
    }
    public Key(float x, float y)
    {
        _x = Mathf.RoundToInt(x);
        _y = Mathf.RoundToInt(y);
    }

    public bool Equals(Key other) => other is not null && _x == other._x && _y == other._y;
    public override bool Equals(object obj) => Equals(obj as Key);

    public override int GetHashCode() => HashCode.Combine(_x, _y);

    public static Key operator +(Key a, Key b) => new(a._x + b._x, a._y + b._y);
    public static Key operator -(Key a, Key b) => new(a._x - b._x, a._y - b._y);
    public static Key operator -(Key a) => new(-a._x, -a._y);
    public static KeyDouble operator &(Key a, Key b) => new(a, b);

    public static bool operator ==(Key a, Key b) => (a is null && b is null) || (a is not null && a.Equals(b));
    public static bool operator !=(Key a, Key b) => !(a == b);
    public override string ToString() => $"({_x}, {_y})";
}
