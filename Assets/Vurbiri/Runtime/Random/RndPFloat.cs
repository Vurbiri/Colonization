using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable, JsonConverter(typeof(RndPFloat.Converter))]
	public struct RndPFloat
	{
		[SerializeField] private float _max;

		public readonly float Min { [Impl(256)] get => 0f; }
		public readonly float Max { [Impl(256)] get => _max; }
		public readonly float Delta { [Impl(256)] get => _max; }
		public readonly float Avg { [Impl(256)] get => _max * 0.5f; }

		public readonly float Roll { [Impl(256)] get => Random.Range(0f, _max); }
		public readonly float RollMoreAvg { [Impl(256)] get => Random.Range(Avg, _max); }
		public readonly float RollLessAvg { [Impl(256)] get => Random.Range(0f, Avg); }

		[Impl(256)] public RndPFloat(float maxInclusive)
		{
			if (maxInclusive > 0f)
				_max = maxInclusive;
			else
				_max = -maxInclusive;
		}

		[Impl(256)] public static float Next(float maxInclusive) => Random.Range(0f, System.MathF.Abs(maxInclusive));

		[Impl(256)] public static implicit operator RndPFloat(float value) => new(value);
		[Impl(256)] public static implicit operator float(RndPFloat rnd) => rnd.Roll;
		[Impl(256)] public static explicit operator int(RndPFloat rnd) => (int)rnd.Roll;

		[Impl(256)] public static RndPFloat operator *(RndPFloat rnd1, RndPFloat rnd2) => new(rnd1._max * rnd2._max);
		[Impl(256)] public static RndPFloat operator /(RndPFloat rnd1, RndPFloat rnd2) => new(rnd1._max / rnd2._max);
		[Impl(256)] public static RndPFloat operator +(RndPFloat rnd1, RndPFloat rnd2) => new(rnd1._max + rnd2._max);
		[Impl(256)] public static RndPFloat operator -(RndPFloat rnd1, RndPFloat rnd2) => new(rnd1._max - rnd2._max);

		[Impl(256)] public static float operator *(float value, RndPFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static float operator *(RndPFloat rnd, float value) => rnd.Roll * value;
		[Impl(256)] public static Vector3 operator *(Vector3 value, RndPFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector3 operator *(RndPFloat rnd, Vector3 value) => rnd.Roll * value;
		[Impl(256)] public static Vector2 operator *(Vector2 value, RndPFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector2 operator *(RndPFloat rnd, Vector2 value) => rnd.Roll * value;

		[Impl(256)] public static float operator /(float value, RndPFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static float operator /(RndPFloat rnd, float value) => rnd.Roll / value;
		[Impl(256)] public static Vector3 operator /(Vector3 value, RndPFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static Vector2 operator /(Vector2 value, RndPFloat rnd) => value / rnd.Roll;

		[Impl(256)] public static float operator +(float value, RndPFloat rnd) => value + rnd.Roll;
		[Impl(256)] public static float operator +(RndPFloat rnd, float value) => rnd.Roll + value;

		[Impl(256)] public static float operator -(float value, RndPFloat rnd) => value - rnd.Roll;
		[Impl(256)] public static float operator -(RndPFloat rnd, float value) => rnd.Roll - value;

		[Impl(256)] public override readonly string ToString() => $"[0, {_max}]";

		#region Nested Json Converter
		//***************************************************************************
		sealed private class Converter : AJsonConverter<RndPFloat>
		{
			public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
			{
				return new RndPFloat(serializer.Deserialize<float>(reader));
			}

			protected override void WriteJson(JsonWriter writer, RndPFloat value, JsonSerializer serializer)
			{
				writer.WriteValue(value._max);
			}
		}
		#endregion
	}
}
