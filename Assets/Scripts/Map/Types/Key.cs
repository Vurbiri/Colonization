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
    public Key(Vector3 v3, Vector2 v2)
    {
        _x = Mathf.RoundToInt(v3.x / v2.x);
        _y = Mathf.RoundToInt(v3.z / v2.y);
    }
    public Key(Vector3 v3, Vector2 v2, float f)
    {
        _x = Mathf.RoundToInt(f * v3.x / v2.x);
        _y = Mathf.RoundToInt(v3.z / v2.y);
    }

    public static Key FromVectors(Vector3 v3, Vector2 v2) => new(v3, v2);
    public static Key FromVectorsRate(Vector3 v3, Vector2 v2) => new(v3, v2, 2f);

    public bool Equals(Key other) => this == other;
    public override bool Equals(object obj) => obj as Key  == this;

    public override int GetHashCode() => HashCode.Combine(_x, _y);

    public static Key operator +(Key a, Key b) => new(a._x + b._x, a._y + b._y);
    public static KeyDouble operator &(Key a, Key b) => new(a, b);

    public static bool operator ==(Key lhs, Key rhs) => (lhs is null && rhs is null) || (lhs is not null && rhs is not null && lhs._x == rhs._x && lhs._y == rhs._y);
    public static bool operator !=(Key lhs, Key rhs) => !(lhs == rhs);

    public override string ToString() => $"({_x}, {_y})";
}
