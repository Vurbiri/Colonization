using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    [Serializable, JsonConverter(typeof(Converter))]
    public class RefInt : RefValue<int>
	{
        [Impl(256)] public RefInt() { }
        [Impl(256)] public RefInt(int value) => _value = value;

        #region Arithmetic
        [Impl(256)] public void Increment() => ++_value;
        [Impl(256)] public void Decrement() => --_value;
        [Impl(256)] public void Add(int value) => _value += value;
        [Impl(256)] public void Remove(int value) => _value -= value;
        [Impl(256)] public void Multiply(int value) => _value *= value;
        [Impl(256)] public void Divide(int value) => _value /= value;
        #endregion

        [Impl(256)] public static implicit operator int(RefInt value) => value._value;
        [Impl(256)] public static implicit operator float(RefInt value) => value._value;

        #region Arithmetic operator
        [Impl(256)] public static int operator +(RefInt r) => r._value;
        [Impl(256)] public static int operator +(RefInt a, RefInt b) => a._value + b._value;
        [Impl(256)] public static int operator +(RefInt r, int i) => r._value + i;
        [Impl(256)] public static int operator +(int i, RefInt r) => i + r._value;
        [Impl(256)] public static float operator +(RefInt r, float f) => r._value + f;
        [Impl(256)] public static float operator +(float f, RefInt r) => f + r._value;

        [Impl(256)] public static int operator -(RefInt r) => -r._value;
        [Impl(256)] public static int operator -(RefInt a, RefInt b) => a._value - b._value;
        [Impl(256)] public static int operator -(RefInt r, int i) => r._value - i;
        [Impl(256)] public static int operator -(int i, RefInt r) => i - r._value;
        [Impl(256)] public static float operator -(RefInt r, float f) => r._value - f;
        [Impl(256)] public static float operator -(float f, RefInt r) => f - r._value;

        [Impl(256)] public static int operator *(RefInt a, RefInt b) => a._value * b._value;
        [Impl(256)] public static int operator *(RefInt r, int i) => r._value * i;
        [Impl(256)] public static int operator *(int i, RefInt r) => i * r._value;
        [Impl(256)] public static float operator *(RefInt r, float f) => r._value * f;
        [Impl(256)] public static float operator *(float f, RefInt r) => f * r._value;

        [Impl(256)] public static int operator /(RefInt a, RefInt b) => a._value / b._value;
        [Impl(256)] public static int operator /(RefInt r, int i) => r._value / i;
        [Impl(256)] public static int operator /(int i, RefInt r) => i / r._value;
        [Impl(256)] public static float operator /(RefInt r, float f) => r._value / f;
        [Impl(256)] public static float operator /(float f, RefInt r) => f / r._value;

        [Impl(256)] public static int operator %(RefInt a, RefInt b) => a._value % b._value;
        [Impl(256)] public static int operator %(RefInt r, int i) => r._value % i;
        [Impl(256)] public static int operator %(int i, RefInt r) => i % r._value;
        #endregion

        #region Logic operator
        [Impl(256)] public static int operator ~(RefInt r) => ~r._value;

        [Impl(256)] public static int operator |(RefInt a, RefInt b) => a._value | b._value;
        [Impl(256)] public static int operator |(RefInt r, int i) => r._value | i;
        [Impl(256)] public static int operator |(int i, RefInt r) => i | r._value;

        [Impl(256)] public static int operator &(RefInt a, RefInt b) => a._value & b._value;
        [Impl(256)] public static int operator &(RefInt r, int i) => r._value & i;
        [Impl(256)] public static int operator &(int i, RefInt r) => i & r._value;

        [Impl(256)] public static int operator ^(RefInt a, RefInt b) => a._value ^ b._value;
        [Impl(256)] public static int operator ^(RefInt r, int i) => r._value ^ i;
        [Impl(256)] public static int operator ^(int i, RefInt r) => i ^ r._value;

        [Impl(256)] public static int operator >>(RefInt r, int i) => r._value >> i;
        [Impl(256)] public static int operator <<(RefInt r, int i) => r._value << i;
        #endregion

        #region Nested JsonConverter
        //***************************************************
        sealed public class Converter : AJsonConverter<RefInt>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new RefInt(serializer.Deserialize<int>(reader));
            }

            protected override void WriteJson(JsonWriter writer, RefInt value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
    }
}
