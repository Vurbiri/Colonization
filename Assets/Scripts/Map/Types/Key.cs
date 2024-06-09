using Newtonsoft.Json;
using System;
using UnityEngine;

public class Key : IEquatable<Key>
{
    [JsonProperty("x")]
    public int X => _x;
    [JsonProperty("y")]
    public int Y => _y;

    private readonly int _x, _y;

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

    public bool Equals(Key other) => this == other;
    public override bool Equals(object obj) => obj as Key  == this;

    public override int GetHashCode() => HashCode.Combine(_x, _y);

    public static Key operator +(Key a, Key b) => new(a._x + b._x, a._y + b._y);
    public static Key operator -(Key a, Key b) => new(a._x - b._x, a._y - b._y);
    public static KeyDouble operator &(Key a, Key b) => new(a, b);

    public static bool operator ==(Key a, Key b) => (a is null && b is null) || (a is not null && b is not null && a._x == b._x && a._y == b._y);
    public static bool operator !=(Key a, Key b) => !(a == b);
    public override string ToString() => $"({_x}, {_y})";
}
