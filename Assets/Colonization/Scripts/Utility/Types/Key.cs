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

        public static readonly Key Zero = new();

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
			this.x = x.Round();
			this.y = y.Round();
		}
		[Impl(256)] public Key(int[] arr)
		{
            x = arr[0];
            y = arr.Length == 2 ? arr[1] : x;
		}

		[Impl(256)] public Key Abs() => new(MathI.Abs(x), MathI.Abs(y));

		[Impl(256)] public readonly string ToSaveKey() => x != y ? string.Concat(x.ToStr(), y.ToStr()) : x.ToStr();

		[Impl(256)] public readonly bool Equals(Key other) => x == other.x & y == other.y;
		[Impl(256)] public override readonly bool Equals(object obj) => obj is Key key && x == key.x & y == key.y;

		[Impl(256)] public override readonly int GetHashCode() => HashCode.Combine(x, y);

		[Impl(256)] public static Key operator +(Key a, Key b) => new(a.x + b.x, a.y + b.y);
		[Impl(256)] public static Key operator -(Key a, Key b) => new(a.x - b.x, a.y - b.y);
		[Impl(256)] public static Key operator -(Key a) => new(-a.x, -a.y);

		[Impl(256)] public static Key operator *(Key a, Key b) => new(a.x * b.x, a.y * b.y);
		[Impl(256)] public static Key operator /(Key a, Key b) => new(a.x / b.x, a.y / b.y);

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
				if (obj is not Key key)
					throw Errors.JsonSerialization(_type);

				WriteToArray(writer, key);
			}

			[Impl(256)] public static void WriteToArray(JsonWriter writer, Key key)
			{
				writer.WriteStartArray();
				writer.WriteValue(key.x);
				if(key.x != key.y)
					writer.WriteValue(key.y);
				writer.WriteEndArray();
			}
		}
		#endregion
	}

}
