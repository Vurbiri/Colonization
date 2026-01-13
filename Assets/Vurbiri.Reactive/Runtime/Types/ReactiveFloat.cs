using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
	[Serializable, JsonConverter(typeof(Converter))]
	sealed public class ReactiveFloat : ReactiveValue<float>
	{
        [Impl(256)] public ReactiveFloat() { }
        [Impl(256)] public ReactiveFloat(float value) => _value = value;

		#region Arithmetic
		[Impl(256)] public void Add(float value)
		{
			if (value != 0f)
				_onChange.Invoke(_value += value);
		}
		[Impl(256)] public void Remove(float value)
		{
			if (value != 0f)
				_onChange.Invoke(_value -= value);
		}
		[Impl(256)] public void Multiply(float value)
		{
			if (value != 1f)
				_onChange.Invoke(_value *= value);
		}
		[Impl(256)] public void Divide(float value)
		{
			if (value != 1f)
				_onChange.Invoke(_value /= value);
		}
        #endregion

        #region Silent Arithmetic
        [Impl(256)] public void SilentAdd(float value = 1f) => _value += value;
        [Impl(256)] public void SilentRemove(float value = 1f) => _value -= value;
        [Impl(256)] public void SilentMultiply(float value) => _value *= value;
        [Impl(256)] public void SilentDivide(float value) => _value /= value;
        #endregion

        [Impl(256)] public static implicit operator float(ReactiveFloat value) => value._value;
        [Impl(256)] public static explicit operator int(ReactiveFloat value) => (int)value._value;

        #region Arithmetic operator
        [Impl(256)] public static float operator +(ReactiveFloat r) => r._value;
        [Impl(256)] public static float operator +(ReactiveFloat a, ReactiveFloat b) => a._value + b._value;
        [Impl(256)] public static float operator +(ReactiveFloat r, float f) => r._value + f;
        [Impl(256)] public static float operator +(float f, ReactiveFloat r) => f + r._value;

        [Impl(256)] public static float operator -(ReactiveFloat r) => -r._value;
        [Impl(256)] public static float operator -(ReactiveFloat a, ReactiveFloat b) => a._value - b._value;
        [Impl(256)] public static float operator -(ReactiveFloat r, float f) => r._value - f;
        [Impl(256)] public static float operator -(float f, ReactiveFloat r) => f - r._value;

        [Impl(256)] public static float operator *(ReactiveFloat a, ReactiveFloat b) => a._value * b._value;
        [Impl(256)] public static float operator *(ReactiveFloat r, float f) => r._value * f;
        [Impl(256)] public static float operator *(float f, ReactiveFloat r) => f * r._value;

        [Impl(256)] public static float operator /(ReactiveFloat a, ReactiveFloat b) => a._value / b._value;
        [Impl(256)] public static float operator /(ReactiveFloat r, float f) => r._value / f;
        [Impl(256)] public static float operator /(float f, ReactiveFloat r) => f / r._value;
		#endregion

		#region Nested JsonConverter
		//***************************************************
		sealed public class Converter : AJsonConverter<ReactiveFloat>
		{
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				return new ReactiveFloat(serializer.Deserialize<float>(reader));
			}

			protected override void WriteJson(JsonWriter writer, ReactiveFloat value, JsonSerializer serializer)
			{
				writer.WriteValue(value._value);
			}
		}
		#endregion
	}
}
