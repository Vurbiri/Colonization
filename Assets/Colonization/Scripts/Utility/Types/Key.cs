//Assets\Colonization\Scripts\Utility\Types\Key.cs
using Newtonsoft.Json;
using System;
using UnityEngine;
using Vurbiri.Colonization;

[JsonObject(MemberSerialization.OptIn)]
public struct Key : IEquatable<Key>, IArrayable
{
    [JsonProperty("x")]
    private int _x;
    [JsonProperty("y")]
    private int _y;

    private static Key _zero = new();
    public static Key Zero => _zero;
    
    public readonly int X => _x;
    public readonly int Y => _y;

    public readonly int Distance
    {
        get
        {
            int x = Mathf.Abs(_x), y = Mathf.Abs(_y);
            if (y - x < 0) return (x + y) >> 1;
            return y;
        }
    }

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

    #region IArrayable
    private const int SIZE_ARRAY = 2;
    public readonly int[] ToArray() => new int[] { _x, _y };
    public readonly int[] ToArray(int[] array)
    {
        if (array == null || array.Length != SIZE_ARRAY)
            return new int[] { _x, _y };

        int i = 0;
        array[i++] = _x; array[i] = _y;
        return array;
    }
    #endregion

    public readonly string ToSaveKey(string separator) => $"{_x}{separator}{_y}";
    public readonly string ToSaveKey() => $"{_x}{_y}";

    public readonly bool Equals(Key other) => _x == other._x & _y == other._y;
    public override readonly bool Equals(object obj) => obj is Key key && _x == key._x & _y == key._y;

    public override readonly int GetHashCode() => HashCode.Combine(_x, _y);

    public static Key operator +(Key a, Key b) => new(a._x + b._x, a._y + b._y);
    public static Key operator -(Key a, Key b) => new(a._x - b._x, a._y - b._y);
    public static Key operator -(Key a) => new(-a._x, -a._y);

    public static Key operator *(Key k, int i) => new(k._x * i, k._y * i);
    public static Key operator *(int i, Key k) => new(k._x * i, k._y * i);

    public static bool operator ==(Key a, Key b) => a._x == b._x & a._y == b._y;
    public static bool operator !=(Key a, Key b) => a._x != b._x | a._y != b._y;

    public override readonly string ToString() => $"[{_x}, {_y}]";

}
