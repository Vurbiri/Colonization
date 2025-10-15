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
            this.x = MathI.Round(x);
            this.y = MathI.Round(y);
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
        sealed public class Converter : JsonConverter
        {
            private readonly Type _type = typeof(Key);

            public override bool CanConvert(Type objectType) => _type == objectType;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new Key(serializer.Deserialize<int[]>(reader));
            }

            sealed public override void WriteJson(JsonWriter writer, object obj, JsonSerializer serializer)
            {
                if (obj is not Key value)
                    throw Errors.JsonSerialization(_type);

                WriteToArray(writer, value);
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
