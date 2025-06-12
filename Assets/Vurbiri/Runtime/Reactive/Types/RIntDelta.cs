using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Reactive
{
    sealed public class RIntDelta : IReactive<int, int>, IEquatable<int>, IEquatable<RIntDelta>, IComparable<int>, IComparable<RIntDelta>
    {
        private int _value;
        private readonly Subscription<int, int> _subscriber = new();

        public int Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value;
            set 
            {
                if (_value != value)
                {
                    int oldValue = _value;
                    _subscriber.Invoke(_value = value, value - oldValue);
                }
            }
        }

        public RIntDelta(int value) => _value = value;

        public void Add(int delta)
        {
            if (delta != 0)
                _subscriber.Invoke(_value += delta, delta);
        }

        public Unsubscription Subscribe(Action<int, int> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, _value, 0);

        public static implicit operator RIntDelta(int value) => new(value);

        public bool Equals(int value) => _value == value;
        public bool Equals(RIntDelta other) => other is not null &&  _value == other._value;
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other is int intValue) return _value == intValue;
            if (other is RIntDelta rValue) return _value == rValue._value;
            return false;
        }

        public int CompareTo(int value) => _value - value;
        public int CompareTo(RIntDelta other) => _value - other._value;

        public override int GetHashCode()=> _value;

        public static bool operator ==(RIntDelta a, RIntDelta b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value == b._value);
        public static bool operator !=(RIntDelta a, RIntDelta b) => !(a == b);
        public static bool operator ==(RIntDelta a, int i) => a is not null && a._value == i;
        public static bool operator !=(RIntDelta a, int i) => a is null || a._value == i;
        public static bool operator ==(int i, RIntDelta a) => a is not null && a._value == i;
        public static bool operator !=(int i, RIntDelta a) => a is null || a._value == i;

        public static bool operator >(RIntDelta a, RIntDelta b) => a._value > b._value;
        public static bool operator >=(RIntDelta a, RIntDelta b) => a._value >= b._value;
        public static bool operator <(RIntDelta a, RIntDelta b) => a._value < b._value;
        public static bool operator <=(RIntDelta a, RIntDelta b) => a._value >= b._value;

        public static bool operator >(RIntDelta a, int i) => a._value > i;
        public static bool operator >=(RIntDelta a, int i) => a._value >= i;
        public static bool operator <(RIntDelta a, int i) => a._value < i;
        public static bool operator <=(RIntDelta a, int i) => a._value <= i;

        public static bool operator >(int i, RIntDelta a) => i > a._value;
        public static bool operator >=(int i, RIntDelta a) => i >= a._value;
        public static bool operator <(int i, RIntDelta a) => i < a._value;
        public static bool operator <=(int i, RIntDelta a) => i <= a._value;
    }
}
