using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[Serializable, JsonConverter(typeof(Converter))]
	sealed public class RefFloat : RefValue<float>
    {
		[Impl(256)] public RefFloat() { }
		[Impl(256)] public RefFloat(float value) => _value = value;

        #region Arithmetic
        [Impl(256)] public void Add(float value = 1f) => _value += value;
        [Impl(256)] public void Remove(float value = 1f) => _value -= value;
        [Impl(256)] public void Multiply(float value) => _value *= value;
        [Impl(256)] public void Divide(float value) => _value /= value;
        #endregion

        [Impl(256)] public static implicit operator float(RefFloat value) => value._value;
        [Impl(256)] public static explicit operator int(RefFloat value) => (int)value._value;

        #region Arithmetic operator
        [Impl(256)] public static float operator +(RefFloat r) => r._value;
        [Impl(256)] public static float operator +(RefFloat a, RefFloat b) => a._value + b._value;
        [Impl(256)] public static float operator +(RefFloat r, float v) => r._value + v;
        [Impl(256)] public static float operator +(float v, RefFloat r) => v + r._value;

        [Impl(256)] public static float operator -(RefFloat r) => -r._value;
        [Impl(256)] public static float operator -(RefFloat a, RefFloat b) => a._value - b._value;
        [Impl(256)] public static float operator -(RefFloat r, float v) => r._value - v;
        [Impl(256)] public static float operator -(float v, RefFloat r) => v - r._value;

        [Impl(256)] public static float operator *(RefFloat a, RefFloat b) => a._value * b._value;
        [Impl(256)] public static float operator *(RefFloat r, float v) => r._value * v;
        [Impl(256)] public static float operator *(float v, RefFloat r) => v * r._value;

        [Impl(256)] public static float operator /(RefFloat a, RefFloat b) => a._value / b._value;
        [Impl(256)] public static float operator /(RefFloat r, float v) => r._value / v;
        [Impl(256)] public static float operator /(float v, RefFloat r) => v / r._value;
        #endregion

        #region Nested JsonConverter
        //***************************************************
        sealed public class Converter : AJsonConverter<RefFloat>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new RefFloat(serializer.Deserialize<float>(reader));
            }

            protected override void WriteJson(JsonWriter writer, RefFloat value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
    }
}
