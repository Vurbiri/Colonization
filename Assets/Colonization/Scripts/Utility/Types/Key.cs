using Newtonsoft.Json;
using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [JsonConverter(typeof(Converter))]
    public readonly struct Key : IEquatable<Key>
    {
        public readonly int x, y;

        private static readonly Key s_zero = new();
        public static Key Zero => s_zero;

        [Impl(256)] public Key(int x, int y)
        {
            this.x = x; this.y = y;
        }
        [Impl(256)] public Key(Vector2Int vector)
        {
            x = vector.x; y = vector.y;
        }
        [Impl(256)] public Key(float x, float y)
        {
            this.x = MathI.RoundToInt(x);
            this.y = MathI.RoundToInt(y);
        }
        [Impl(256)] public Key(int[] arr)
        {
            x = arr[0];
            y = arr[1];
        }

        [Impl(256)] public Key Abs() => new(Math.Abs(x), Math.Abs(y));

        [Impl(256)] public readonly string ToSaveKey() => $"{x}{y}";

        [Impl(256)] public readonly bool Equals(Key other) => x == other.x & y == other.y;
        [Impl(256)] public override readonly bool Equals(object obj) => obj is Key key && x == key.x & y == key.y;

        [Impl(256)] public override readonly int GetHashCode() => HashCode.Combine(x, y);

        [Impl(256)] public static Key operator +(Key a, Key b) => new(a.x + b.x, a.y + b.y);
        [Impl(256)] public static Key operator -(Key a, Key b) => new(a.x - b.x, a.y - b.y);
        [Impl(256)] public static Key operator -(Key a) => new(-a.x, -a.y);

        [Impl(256)] public static bool operator ==(Key a, Key b) => a.x == b.x & a.y == b.y;
        [Impl(256)] public static bool operator !=(Key a, Key b) => a.x != b.x | a.y != b.y;

        public override readonly string ToString() => $"[{x}, {y}]";

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
                WriteToArray(writer ,value);
            }

            [Impl(256)] public static void WriteToArray(JsonWriter writer, Key value)
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
