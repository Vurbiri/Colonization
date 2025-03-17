//Assets\Vurbiri\Runtime\Reactive\Types\RInt.cs
using Newtonsoft.Json;
using System;

namespace Vurbiri.Reactive
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    sealed public class RInt : ARType<int>
    {
        public RInt() : base(0) { }
        public RInt(int value) : base(value) { }


        #region Arithmetic
        public void Increment() => _subscriber.Invoke(++_value);
        public void Decrement() => _subscriber.Invoke(--_value);
        public void Add(int value)
        {
            if (value != 0)
                _subscriber.Invoke(_value += value);
        }
        public void Remove(int value)
        {
            if (value != 0)
                _subscriber.Invoke(_value -= value);
        }
        public void Multiply(int value)
        {
            if (value != 1)
                _subscriber.Invoke(_value *= value);
        }
        public void Divide(int value)
        {
            if (value != 1)
                _subscriber.Invoke(_value /= value);
        }
        #endregion

        public static explicit operator RInt(int value) => new(value);

        public static implicit operator int(RInt value) => value._value;
        public static implicit operator float(RInt value) => value._value;

        #region Arithmetic operator
        public static int operator +(RInt a, RInt b) => a._value + b._value;
        public static int operator +(RInt r, IReactiveValue<int> i) => r._value + i.Value;
        public static int operator +(IReactiveValue<int> i, RInt r) => i.Value + r._value;
        public static int operator +(RInt r, int i) => r._value + i;
        public static int operator +(int i, RInt r) => i + r._value;
        public static float operator +(RInt r, float f) => r._value + f;
        public static float operator +(float f, RInt r) => f + r._value;
        
        public static RInt operator ++(RInt r) { r._subscriber.Invoke(++r._value); return r; }

        public static int operator -(RInt a, RInt b) => a._value - b._value;
        public static int operator -(RInt r, IReactiveValue<int> i) => r._value - i.Value;
        public static int operator -(IReactiveValue<int> i, RInt r) => i.Value - r._value;
        public static int operator -(RInt r, int i) => r._value - i;
        public static int operator -(int i, RInt r) => i - r._value;
        public static float operator -(RInt r, float f) => r._value - f;
        public static float operator -(float f, RInt r) => f - r._value;
        
        public static RInt operator --(RInt r) { r._subscriber.Invoke(--r._value); return r; }

        public static int operator *(RInt a, RInt b) => a._value * b._value;
        public static int operator *(RInt r, IReactiveValue<int> i) => r._value * i.Value;
        public static int operator *(IReactiveValue<int> i, RInt r) => i.Value * r._value;
        public static int operator *(RInt r, int i) => r._value * i;
        public static int operator *(int i, RInt r) => i * r._value;
        public static float operator *(RInt r, float f) => r._value * f;
        public static float operator *(float f, RInt r) => f * r._value;
       
        public static int operator /(RInt a, RInt b) => a._value / b._value;
        public static int operator /(RInt r, IReactiveValue<int> i) => r._value / i.Value;
        public static int operator /(IReactiveValue<int> i, RInt r) => i.Value / r._value;
        public static int operator /(RInt r, int i) => r._value / i;
        public static int operator /(int i, RInt r) => i / r._value;
        public static float operator /(RInt r, float f) => r._value / f;
        public static float operator /(float f, RInt r) => f / r._value;

        #endregion
    }
}
