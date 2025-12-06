using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
    [Serializable, JsonConverter(typeof(Converter))]
    sealed public class RInt : AReactiveType<int>
    {
        [Impl(256)] public RInt() : base(0) { }
        [Impl(256)] public RInt(int value) : base(value) { }

        #region Arithmetic
        [Impl(256)] public void Increment() => _onChange.Invoke(++_value);
        [Impl(256)] public void Decrement() => _onChange.Invoke(--_value);
        [Impl(256)] public void Add(int value)
        {
            if (value != 0)
                _onChange.Invoke(_value += value);
        }
        [Impl(256)] public void Remove(int value)
        {
            if (value != 0)
                _onChange.Invoke(_value -= value);
        }
        [Impl(256)] public void Multiply(int value)
        {
            if (value != 1)
                _onChange.Invoke(_value *= value);
        }
        [Impl(256)] public void Divide(int value)
        {
            if (value != 1)
                _onChange.Invoke(_value /= value);
        }
        #endregion

        #region Silent Arithmetic
        [Impl(256)] public void SilentAdd(int value = 1) => _value += value;
        [Impl(256)] public void SilentRemove(int value = 1) => _value -= value;
        [Impl(256)] public void SilentMultiply(int value) => _value *= value;
        [Impl(256)] public void SilentDivide(int value) => _value /= value;
        #endregion

        [Impl(256)] public static implicit operator int(RInt value) => value._value;
        [Impl(256)] public static implicit operator float(RInt value) => value._value;

        #region Arithmetic operator
        [Impl(256)] public static int operator +(RInt a, RInt b) => a._value + b._value;
        [Impl(256)] public static int operator +(RInt r, ReactiveValue<int> i) => r._value + i.Value;
        [Impl(256)] public static int operator +(ReactiveValue<int> i, RInt r) => i.Value + r._value;
        [Impl(256)] public static int operator +(RInt r, int i) => r._value + i;
        [Impl(256)] public static int operator +(int i, RInt r) => i + r._value;
        [Impl(256)] public static float operator +(RInt r, float f) => r._value + f;
        [Impl(256)] public static float operator +(float f, RInt r) => f + r._value;

        [Impl(256)] public static int operator -(RInt a, RInt b) => a._value - b._value;
        [Impl(256)] public static int operator -(RInt r, ReactiveValue<int> i) => r._value - i.Value;
        [Impl(256)] public static int operator -(ReactiveValue<int> i, RInt r) => i.Value - r._value;
        [Impl(256)] public static int operator -(RInt r, int i) => r._value - i;
        [Impl(256)] public static int operator -(int i, RInt r) => i - r._value;
        [Impl(256)] public static float operator -(RInt r, float f) => r._value - f;
        [Impl(256)] public static float operator -(float f, RInt r) => f - r._value;

        [Impl(256)] public static int operator *(RInt a, RInt b) => a._value * b._value;
        [Impl(256)] public static int operator *(RInt r, ReactiveValue<int> i) => r._value * i.Value;
        [Impl(256)] public static int operator *(ReactiveValue<int> i, RInt r) => i.Value * r._value;
        [Impl(256)] public static int operator *(RInt r, int i) => r._value * i;
        [Impl(256)] public static int operator *(int i, RInt r) => i * r._value;
        [Impl(256)] public static float operator *(RInt r, float f) => r._value * f;
        [Impl(256)] public static float operator *(float f, RInt r) => f * r._value;

        [Impl(256)] public static int operator /(RInt a, RInt b) => a._value / b._value;
        [Impl(256)] public static int operator /(RInt r, ReactiveValue<int> i) => r._value / i.Value;
        [Impl(256)] public static int operator /(ReactiveValue<int> i, RInt r) => i.Value / r._value;
        [Impl(256)] public static int operator /(RInt r, int i) => r._value / i;
        [Impl(256)] public static int operator /(int i, RInt r) => i / r._value;
        [Impl(256)] public static float operator /(RInt r, float f) => r._value / f;
        [Impl(256)] public static float operator /(float f, RInt r) => f / r._value;

        #endregion

        #region Nested JsonConverter
        //***************************************************
        sealed public class Converter : AJsonConverter<RInt>
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new RInt(serializer.Deserialize<int>(reader));
            }

            protected override void WriteJson(JsonWriter writer, RInt value, JsonSerializer serializer)
            {
                writer.WriteValue(value._value);
            }
        }
        #endregion
    }
}
