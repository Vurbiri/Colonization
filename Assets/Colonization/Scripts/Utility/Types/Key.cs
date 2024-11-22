//Assets\Colonization\Scripts\Utility\Types\Key.cs
using Newtonsoft.Json;
using System;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public struct Key : IEquatable<Key>
{
    public readonly int X => _x;
    public readonly int Y => _y;

    [JsonProperty("x")]
    private int _x;
    [JsonProperty("y")]
    private int _y;

    public Key(int x, int y)
    {
        _x = x; _y = y;
    }
    public Key(float x, float y)
    {
        _x = Mathf.RoundToInt(x);
        _y = Mathf.RoundToInt(y);
    }
    public Key(int[] arr)
    {
        _x = arr[0];
        _y = arr[1];
    }

    public Key SetValues(int x, int y)
    {
        _x = x; _y = y;

        return this;
    }
    public Key SetValues(int[] arr)
    {
        _x = arr[0];
        _y = arr[1];

        return this;
    }

    public readonly int[] ToArray() => new int[] { _x, _y };

    public readonly bool Equals(Key other) => _x == other._x && _y == other._y;
    public override readonly bool Equals(object obj) => obj is Key key && Equals(key);

    public override readonly int GetHashCode() => HashCode.Combine(_x, _y);

    public static Key operator +(Key a, Key b) => new(a._x + b._x, a._y + b._y);
    public static Key operator -(Key a, Key b) => new(a._x - b._x, a._y - b._y);
    public static Key operator -(Key a) => new(-a._x, -a._y);

    public static bool operator ==(Key a, Key b) => a._x == b._x && a._y == b._y;
    public static bool operator !=(Key a, Key b) => a._x != b._x || a._y != b._y;
    public override readonly string ToString() => $"({_x}, {_y})";
}
