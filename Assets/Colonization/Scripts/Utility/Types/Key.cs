using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{

    [JsonConverter(typeof(Converter))]
    public readonly struct Key : IEquatable<Key>
    {
        private readonly int _x, _y;

        private static readonly Key s_zero = new();
        public static Key Zero => s_zero;

        public readonly int X => _x;
        public readonly int Y => _y;

        public readonly int Distance
        {
            get
            {
                int x = Math.Abs(_x), y = Math.Abs(_y);
                return (y - x < 0) ? (x + y) >> 1 : y;
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

        public readonly string ToSaveKey() => $"{_x}{_y}";

        public readonly bool Equals(Key other) => _x == other._x & _y == other._y;
        public override readonly bool Equals(object obj) => obj is Key key && _x == key._x & _y == key._y;

        public override readonly int GetHashCode() => HashCode.Combine(_x, _y);

        public static Key operator +(Key a, Key b) => new(a._x + b._x, a._y + b._y);
        public static Key operator -(Key a, Key b) => new(a._x - b._x, a._y - b._y);
        public static Key operator -(Key a) => new(-a._x, -a._y);

        public static int operator ^(Key a, Key b)
        {
            int x = Math.Abs(a._x - b._x), y = Math.Abs(a._y - b._y);
            return (y - x < 0) ? (x + y) >> 1 : y;
        }

        public static Key operator *(Key k, int i) => new(k._x * i, k._y * i);
        public static Key operator *(int i, Key k) => new(k._x * i, k._y * i);

        public static bool operator ==(Key a, Key b) => a._x == b._x & a._y == b._y;
        public static bool operator !=(Key a, Key b) => a._x != b._x | a._y != b._y;

        public override readonly string ToString() => $"[{_x}, {_y}]";

        #region Nested: Converter
        //***********************************
        sealed public class Converter : JsonConverter<Key>
        {
            public override Key ReadJson(JsonReader reader, Type objectType, Key existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new(serializer.Deserialize<int[]>(reader));
            }

            public override void WriteJson(JsonWriter writer, Key value, JsonSerializer serializer)
            {
                WriteJsonArray(writer ,value);
            }

            public static void WriteJsonArray(JsonWriter writer, Key value)
            {
                writer.WriteStartArray();
                writer.WriteValue(value._x);
                writer.WriteValue(value._y);
                writer.WriteEndArray();
            }
        }
        #endregion
    }

}
