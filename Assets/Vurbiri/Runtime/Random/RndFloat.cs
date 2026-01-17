using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable, JsonConverter(typeof(RndFloat.Converter))]
	public struct RndFloat
	{
		[SerializeField] private float _min;
		[SerializeField] private float _max;

		public readonly float Min { [Impl(256)] get => _min; }
		public readonly float Max { [Impl(256)] get => _max; }
		public readonly float Delta { [Impl(256)] get => _max - _min; }
		public readonly float Avg { [Impl(256)] get => (_min + _max) * 0.5f; }

		public readonly float Roll { [Impl(256)] get => Random.Range(_min, _max); }
		public readonly float RollMoreAvg { [Impl(256)] get => Random.Range(Avg, _max); }
		public readonly float RollLessAvg { [Impl(256)] get => Random.Range(_min, Avg); }

		[Impl(256)] public RndFloat(float minInclusive, float maxInclusive)
		{
			if (maxInclusive > minInclusive)
			{
				_min = minInclusive; _max = maxInclusive;
			}
			else
			{
				_min = maxInclusive; _max = minInclusive;
			}
		}
		[Impl(256)] public RndFloat(RndFloat value, float ratio)
		{
			_min = value._min * ratio;
			_max = value._max * ratio;
		}
		[Impl(256)] public RndFloat(Vector2 vector) : this(vector.x, vector.y) { }

		[Impl(256)] public static float Next(float maxInclusive) => Random.Range(0f, maxInclusive);
		[Impl(256)] public static float Next(float minInclusive, float maxInclusive) => Random.Range(minInclusive, maxInclusive);


		[Impl(256)] public static implicit operator float(RndFloat rnd) => rnd.Roll;
		[Impl(256)] public static explicit operator int(RndFloat rnd) => (int)rnd.Roll;

		[Impl(256)] public static RndFloat operator *(RndFloat rnd1, RndFloat rnd2) => new(rnd1._min * rnd2._min, rnd1._max * rnd2._max);
		[Impl(256)] public static RndFloat operator /(RndFloat rnd1, RndFloat rnd2) => new(rnd1._min / rnd2._min, rnd1._max / rnd2._max);
		[Impl(256)] public static RndFloat operator +(RndFloat rnd1, RndFloat rnd2) => new(rnd1._min + rnd2._min, rnd1._max + rnd2._max);
		[Impl(256)] public static RndFloat operator -(RndFloat rnd1, RndFloat rnd2) => new(rnd1._min - rnd2._min, rnd1._max - rnd2._max);

		[Impl(256)] public static float operator *(float value, RndFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static float operator *(RndFloat rnd, float value) =>  rnd.Roll * value;
		[Impl(256)] public static Vector3 operator *(Vector3 value, RndFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector3 operator *(RndFloat rnd, Vector3 value) => rnd.Roll * value;
		[Impl(256)] public static Vector2 operator *(Vector2 value, RndFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector2 operator *(RndFloat rnd, Vector2 value) => rnd.Roll * value;

		[Impl(256)] public static float operator /(float value, RndFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static float operator /(RndFloat rnd, float value) => rnd.Roll / value;
		[Impl(256)] public static Vector3 operator /(Vector3 value, RndFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static Vector2 operator /(Vector2 value, RndFloat rnd) => value / rnd.Roll;

		[Impl(256)] public static float operator +(float value, RndFloat rnd) => value + rnd.Roll;
		[Impl(256)] public static float operator +(RndFloat rnd, float value) => rnd.Roll + value;

		[Impl(256)] public static float operator -(float value, RndFloat rnd) => value - rnd.Roll;
		[Impl(256)] public static float operator -(RndFloat rnd, float value) => rnd.Roll - value;

		[Impl(256)] public override readonly string ToString() => $"[{_min}, {_max}]";

		#region Nested Json Converter
		//***************************************************************************
		sealed private class Converter : AJsonConverter<RndFloat>
		{
			public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
			{
				var data = serializer.Deserialize<int[]>(reader);
				return new RndFloat(data[0], data[1]);
			}

			protected override void WriteJson(JsonWriter writer, RndFloat value, JsonSerializer serializer)
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
