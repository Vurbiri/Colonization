using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable, JsonConverter(typeof(RndInt.Converter))]
	public struct RndInt
	{
		[SerializeField] private int _min;
		[SerializeField] private int _max;

		public readonly int Min { [Impl(256)] get => _min; }
		public readonly int Max { [Impl(256)] get => _max - 1; }
		public readonly int Delta { [Impl(256)] get => _max - _min - 1; }
		public readonly int Avg { [Impl(256)] get => (_min + _max - 1) >> 1; }

		public readonly int Roll { [Impl(256)] get => Random.Range(_min, _max); }
		public readonly int RollMoreAvg { [Impl(256)] get => Random.Range(Avg, _max); }
		public readonly int RollLessAvg { [Impl(256)] get => Random.Range(_min, Avg); }

		[Impl(256)]public RndInt(int minInclusive, int maxInclusive)
		{
			if (maxInclusive > minInclusive)
			{
				_min = minInclusive; _max = maxInclusive + 1;
			}
			else
			{
				_min = maxInclusive; _max = minInclusive + 1;
			}
		}
		[Impl(256)] public RndInt(RndInt value, int ratio)
		{
			_min = value._min * ratio;
			_max = value._max * ratio;
		}
		[Impl(256)] public RndInt(Vector2Int vector) : this(vector.x, vector.y) { }

		[Impl(256)] public static int Next(int maxInclusive) => Random.Range(0, maxInclusive + 1);
		[Impl(256)] public static int Next(int minInclusive, int maxInclusive) => Random.Range(minInclusive, maxInclusive + 1);

		[Impl(256)] public static implicit operator int(RndInt rnd) => rnd.Roll;
		[Impl(256)] public static explicit operator byte(RndInt rnd) => (byte)rnd.Roll;
		[Impl(256)] public static implicit operator float(RndInt rnd) => rnd.Roll;

		[Impl(256)] public static RndInt operator *(RndInt rnd1, RndInt rnd2) => new(rnd1._min * rnd2._min, (rnd1._max - 1) * (rnd2._max - 1));
		[Impl(256)] public static RndInt operator /(RndInt rnd1, RndInt rnd2) => new(rnd1._min / rnd2._min, (rnd1._max - 1) / (rnd2._max - 1));
		[Impl(256)] public static RndInt operator +(RndInt rnd1, RndInt rnd2) => new(rnd1._min + rnd2._min, rnd1._max + rnd2._max - 2);
		[Impl(256)] public static RndInt operator -(RndInt rnd1, RndInt rnd2) => new(rnd1._min - rnd2._min, rnd1._max - rnd2._max);

		[Impl(256)] public static int operator *(int value, RndInt rnd) => value * rnd.Roll;
		[Impl(256)] public static int operator *(RndInt rnd, int value) => rnd.Roll * value;
		[Impl(256)] public static Vector3Int operator *(Vector3Int value, RndInt rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector3Int operator *(RndInt rnd, Vector3Int value) => rnd.Roll * value;
		[Impl(256)] public static Vector2Int operator *(Vector2Int value, RndInt rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector2Int operator *(RndInt rnd, Vector2Int value) => rnd.Roll * value;

		[Impl(256)] public static int operator /(int value, RndInt rnd) => value / rnd.Roll;
		[Impl(256)] public static int operator /(RndInt rnd, int value) => rnd.Roll / value;
		[Impl(256)] public static Vector3Int operator /(Vector3Int value, RndInt rnd) => value / rnd.Roll;
		[Impl(256)] public static Vector2Int operator /(Vector2Int value, RndInt rnd) => value / rnd.Roll;

		[Impl(256)] public static int operator +(int value, RndInt rnd) => value + rnd.Roll;
		[Impl(256)] public static int operator +(RndInt rnd, int value) => rnd.Roll + value;

		[Impl(256)] public static int operator -(int value, RndInt rnd) => value - rnd.Roll;
		[Impl(256)] public static int operator -(RndInt rnd, int value) => rnd.Roll - value;

		[Impl(256)] public static int operator >>(RndInt rnd, int value) => rnd.Roll >> value;
		[Impl(256)] public static int operator <<(RndInt rnd, int value) => rnd.Roll << value;

		[Impl(256)] public override readonly string ToString() => $"[{_min}, {_max - 1}]";

		#region Nested Json Converter
		//***************************************************************************
		sealed private class Converter : AJsonConverter<RndInt>
		{
			public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
			{
				var data = serializer.Deserialize<int[]>(reader);
				var value = new RndInt
				{
					_min = data[0],
					_max = data[1]
				};

				return value;
			}

			protected override void WriteJson(JsonWriter writer, RndInt value, JsonSerializer serializer)
			{
				writer.WriteStartArray();
				writer.WriteValue(value._min);
				writer.WriteValue(value._max);
				writer.WriteEndArray();
			}
		}
		#endregion
	}
}
