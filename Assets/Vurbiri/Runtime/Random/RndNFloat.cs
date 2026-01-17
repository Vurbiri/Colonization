using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable, JsonConverter(typeof(RndNFloat.Converter))]
	public struct RndNFloat
	{
		[SerializeField] private float _min;

		public readonly float Min { [Impl(256)] get => _min; }
		public readonly float Max { [Impl(256)] get => 0f; }
		public readonly float Delta { [Impl(256)] get => -_min; }
		public readonly float Avg { [Impl(256)] get => _min * 0.5f; }

		public readonly float Roll { [Impl(256)] get => Random.Range(_min, 0f); }
		public readonly float RollMoreAvg { [Impl(256)] get => Random.Range(Avg, 0f); }
		public readonly float RollLessAvg { [Impl(256)] get => Random.Range(_min, Avg); }

		[Impl(256)] public RndNFloat(float minInclusive)
		{
			if (minInclusive < 0f)
				_min = minInclusive;
			else
				_min = -minInclusive;
		}

		[Impl(256)] public static float Next(float minInclusive) => Random.Range(-System.MathF.Abs(minInclusive), 0f);

		[Impl(256)] public static implicit operator RndNFloat(float value) => new(value);
		[Impl(256)] public static implicit operator float(RndNFloat rnd) => rnd.Roll;
		[Impl(256)] public static explicit operator int(RndNFloat rnd) => (int)rnd.Roll;

		[Impl(256)] public static RndNFloat operator *(RndNFloat rnd1, RndNFloat rnd2) => new(rnd1._min * rnd2._min);
		[Impl(256)] public static RndNFloat operator /(RndNFloat rnd1, RndNFloat rnd2) => new(rnd1._min / rnd2._min);
		[Impl(256)] public static RndNFloat operator +(RndNFloat rnd1, RndNFloat rnd2) => new(rnd1._min + rnd2._min);
		[Impl(256)] public static RndNFloat operator -(RndNFloat rnd1, RndNFloat rnd2) => new(rnd1._min - rnd2._min);

		[Impl(256)] public static float operator *(float value, RndNFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static float operator *(RndNFloat rnd, float value) => rnd.Roll * value;
		[Impl(256)] public static Vector3 operator *(Vector3 value, RndNFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector3 operator *(RndNFloat rnd, Vector3 value) => rnd.Roll * value;
		[Impl(256)] public static Vector2 operator *(Vector2 value, RndNFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector2 operator *(RndNFloat rnd, Vector2 value) => rnd.Roll * value;

		[Impl(256)] public static float operator /(float value, RndNFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static float operator /(RndNFloat rnd, float value) => rnd.Roll / value;
		[Impl(256)] public static Vector3 operator /(Vector3 value, RndNFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static Vector2 operator /(Vector2 value, RndNFloat rnd) => value / rnd.Roll;

		[Impl(256)] public static float operator +(float value, RndNFloat rnd) => value + rnd.Roll;
		[Impl(256)] public static float operator +(RndNFloat rnd, float value) => rnd.Roll + value;

		[Impl(256)] public static float operator -(float value, RndNFloat rnd) => value - rnd.Roll;
		[Impl(256)] public static float operator -(RndNFloat rnd, float value) => rnd.Roll - value;

		[Impl(256)] public override readonly string ToString() => $"[{_min}, 0]";

		#region Nested Json Converter
		//***************************************************************************
		sealed private class Converter : AJsonConverter<RndNFloat>
		{
			public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
			{
				return new RndNFloat(serializer.Deserialize<float>(reader));
			}

			protected override void WriteJson(JsonWriter writer, RndNFloat value, JsonSerializer serializer)
			{
				writer.WriteValue(value._min);
			}
		}
		#endregion
	}
}
