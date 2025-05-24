using Newtonsoft.Json;
using System;

namespace Vurbiri.Reactive
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    sealed public class RFloat : ARType<float>
    {
        public RFloat() : base(0f) { }
        public RFloat(float value) : base(value) { }

        #region Arithmetic
        public void Add(float value)
        {
            if (value != 0f)
                _subscriber.Invoke(_value += value);
        }
        public void Remove(float value)
        {
            if (value != 0f)
                _subscriber.Invoke(_value -= value);
        }
        public void Multiply(float value)
        {
            if (value != 1f)
                _subscriber.Invoke(_value *= value);
        }
        public void Divide(float value)
        {
            if (value != 1f)
                _subscriber.Invoke(_value /= value);
        }
        #endregion

        public static explicit operator RFloat(float value) => new(value);

        public static implicit operator float(RFloat value) => value._value;


        #region Arithmetic operator
        public static float operator +(RFloat a, RFloat b) => a._value + b._value;
        public static float operator +(RFloat r, IReactiveValue<float> f) => r._value + f.Value;
        public static float operator +(IReactiveValue<float> f, RFloat r) => f.Value + r._value;
        public static float operator +(RFloat r, float f) => r._value + f;
        public static float operator +(float f, RFloat r) => f + r._value;

        public static float operator -(RFloat a, RFloat b) => a._value - b._value;
        public static float operator -(RFloat r, IReactiveValue<float> f) => r._value - f.Value;
        public static float operator -(IReactiveValue<float> f, RFloat r) => f.Value - r._value;
        public static float operator -(RFloat r, float f) => r._value - f;
        public static float operator -(float f, RFloat r) => f - r._value;

        public static float operator *(RFloat a, RFloat b) => a._value * b._value;
        public static float operator *(RFloat r, IReactiveValue<float> f) => r._value * f.Value;
        public static float operator *(IReactiveValue<float> f, RFloat r) => f.Value * r._value;
        public static float operator *(RFloat r, float f) => r._value * f;
        public static float operator *(float f, RFloat r) => f * r._value;

        public static float operator /(RFloat a, RFloat b) => a._value / b._value;
        public static float operator /(RFloat r, IReactiveValue<float> f) => r._value / f.Value;
        public static float operator /(IReactiveValue<float> f, RFloat r) => f.Value / r._value;
        public static float operator /(RFloat r, float f) => r._value / f;
        public static float operator /(float f, RFloat r) => f / r._value;
        #endregion
    }
}
