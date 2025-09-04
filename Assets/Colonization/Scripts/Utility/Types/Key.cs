using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{

    [JsonConverter(typeof(Converter))]
    public readonly struct Key : IEquatable<Key>
    {
        public readonly int x, y;

        private static readonly Key s_zero = new();
        public static Key Zero => s_zero;

        public readonly bool IsZero => x == 0 & y == 0;

        public readonly int Magnitude
        {
            get
            {
                int ax = Math.Abs(x), ay = Math.Abs(y);
                return (ay - ax < 0) ? (ax + ay) >> 1 : ay;
            }
        }

        public Key(int x, int y)
        {
            this.x = x; this.y = y;
        }
        public Key(float x, float y)
        {
            this.x = Mathf.RoundToInt(x);
            this.y = Mathf.RoundToInt(y);
        }
        public Key(int[] arr)
        {
            x = arr[0];
            y = arr[1];
        }

        public readonly string ToSaveKey() => $"{x}{y}";

        public readonly bool Equals(Key other) => x == other.x & y == other.y;
        public override readonly bool Equals(object obj) => obj is Key key && x == key.x & y == key.y;

        public override readonly int GetHashCode() => HashCode.Combine(x, y);

        public static Key operator +(Key a, Key b) => new(a.x + b.x, a.y + b.y);
        public static Key operator -(Key a, Key b) => new(a.x - b.x, a.y - b.y);
        public static Key operator -(Key a) => new(-a.x, -a.y);

        public static int operator ^(Key a, Key b) // Distance
        {
            int x = Math.Abs(a.x - b.x), y = Math.Abs(a.y - b.y);
            return (y - x < 0) ? (x + y) >> 1 : y;
        }

        public static Key operator *(Key k, int i) => new(k.x * i, k.y * i);
        public static Key operator /(int i, Key k) => new(k.x / i, k.y / i);

        public static bool operator ==(Key a, Key b) => a.x == b.x & a.y == b.y;
        public static bool operator !=(Key a, Key b) => a.x != b.x | a.y != b.y;

        public override readonly string ToString() => $"{x},{y}";

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
                writer.WriteValue(value.x);
                writer.WriteValue(value.y);
                writer.WriteEndArray();
            }
        }
        #endregion
    }

}
