using Newtonsoft.Json;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable, JsonConverter(typeof(RndMFloat.Converter))]
	public struct RndMFloat
	{
		[SerializeField] private float _value;

		public readonly float Min { [Impl(256)] get => -_value; }
		public readonly float Max { [Impl(256)] get => _value; }
		public readonly float Delta { [Impl(256)] get => _value * 2f; }
		public readonly float Avg { [Impl(256)] get => 0f; }

		public readonly float Roll { [Impl(256)] get => Random.Range(-_value, _value); }
		public readonly float RollMoreAvg { [Impl(256)] get => Random.Range(0f, _value); }
		public readonly float RollLessAvg { [Impl(256)] get => Random.Range(-_value, 0f); }

		[Impl(256)] public RndMFloat(float maxInclusive)
		{
			if (maxInclusive > 0f)
				_value = maxInclusive;
			else
				_value = -maxInclusive;
		}

		[Impl(256)] public static float Next(float inclusive) => Random.Range(-inclusive, inclusive);

		[Impl(256)] public static implicit operator RndMFloat(float value) => new(value);
		[Impl(256)] public static implicit operator float(RndMFloat rnd) => rnd.Roll;
		[Impl(256)] public static explicit operator int(RndMFloat rnd) => (int)rnd.Roll;

		[Impl(256)] public static RndMFloat operator *(RndMFloat rnd1, RndMFloat rnd2) => new(rnd1._value * rnd2._value);
		[Impl(256)] public static RndMFloat operator /(RndMFloat rnd1, RndMFloat rnd2) => new(rnd1._value / rnd2._value);
		[Impl(256)] public static RndMFloat operator +(RndMFloat rnd1, RndMFloat rnd2) => new(rnd1._value + rnd2._value);
		[Impl(256)] public static RndMFloat operator -(RndMFloat rnd1, RndMFloat rnd2) => new(rnd1._value - rnd2._value);

		[Impl(256)] public static float operator *(float value, RndMFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static float operator *(RndMFloat rnd, float value) => rnd.Roll * value;
		[Impl(256)] public static Vector3 operator *(Vector3 value, RndMFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector3 operator *(RndMFloat rnd, Vector3 value) => rnd.Roll * value;
		[Impl(256)] public static Vector2 operator *(Vector2 value, RndMFloat rnd) => value * rnd.Roll;
		[Impl(256)] public static Vector2 operator *(RndMFloat rnd, Vector2 value) => rnd.Roll * value;

		[Impl(256)] public static float operator /(float value, RndMFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static float operator /(RndMFloat rnd, float value) => rnd.Roll / value;
		[Impl(256)] public static Vector3 operator /(Vector3 value, RndMFloat rnd) => value / rnd.Roll;
		[Impl(256)] public static Vector2 operator /(Vector2 value, RndMFloat rnd) => value / rnd.Roll;

		[Impl(256)] public static float operator +(float value, RndMFloat rnd) => value + rnd.Roll;
		[Impl(256)] public static float operator +(RndMFloat rnd, float value) => rnd.Roll + value;

		[Impl(256)] public static float operator -(float value, RndMFloat rnd) => value - rnd.Roll;
		[Impl(256)] public static float operator -(RndMFloat rnd, float value) => rnd.Roll - value;

		[Impl(256)] public override readonly string ToString() => $"[{-_value}, {_value}]";

		#region Nested Json Converter
		//***************************************************************************
		sealed private class Converter : AJsonConverter<RndMFloat>
		{
			public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
			{
				return new RndMFloat(serializer.Deserialize<float>(reader));
			}

			protected override void WriteJson(JsonWriter writer, RndMFloat value, JsonSerializer serializer)
			{
				writer.WriteValue(value._value);
			}
		}
		#endregion
	}
}
