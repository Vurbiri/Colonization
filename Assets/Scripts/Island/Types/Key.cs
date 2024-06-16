using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Key : IEquatable<Key>
{
    [JsonProperty("x")]
    public int X => _x;
    [JsonProperty("y")]
    public int Y => _y;

    private readonly int _x, _y;

    private static readonly List<Key> _crossroadIndexesOffset = new(6) { new(2, -1), new(2, 1), new(0, 2), new(-2, 1), new(-2, -1), new(0, -2) };

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

    public static implicit operator LinkType(Key key) => (LinkType)(_crossroadIndexesOffset.IndexOf(key) % 3);
    public static implicit operator CrossroadType(Key key) => (CrossroadType)(_crossroadIndexesOffset.IndexOf(key) % 2);

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
