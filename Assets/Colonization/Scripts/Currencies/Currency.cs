using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class Currency : IReactiveValue<int>, IEquatable<Currency>, IComparable<Currency>
    {
        protected int _value;
        protected readonly Subscription<int> _changeValue = new();
        protected readonly Subscription<int> _deltaValue = new();

        public int Value => _value;

        public Unsubscription Subscribe(Action<int> action, bool instantGetValue = true) => _changeValue.Add(action, instantGetValue, _value);
        public Unsubscription SubscribeDelta(Action<int> action) => _deltaValue.Add(action);

        public bool Equals(Currency other) => other is not null && _value == other._value;
        public int CompareTo(Currency other) => -_value.CompareTo(other._value);
        sealed public override bool Equals(object other) => Equals(other as Currency);
        sealed public override int GetHashCode() => _value.GetHashCode();
        sealed public override string ToString() => _value.ToString();

        public static bool operator ==(Currency a, Currency b) => (a is null & b is null) || (a is not null & b is not null && a._value == b._value);
        public static bool operator !=(Currency a, Currency b) => !(a == b);
        public static bool operator >=(Currency a, Currency b) => (a is null & b is null) || (a is not null & b is not null && a._value >= b._value);
        public static bool operator <(Currency a, Currency b) => !(a >= b);
        public static bool operator <=(Currency a, Currency b) => (a is null & b is null) || (a is not null & b is not null && a._value <= b._value);
        public static bool operator >(Currency a, Currency b) => !(a <= b);

        public static bool operator ==(Currency a, int b) => a is not null && a._value == b;
        public static bool operator !=(Currency a, int b) => a is null || a._value != b;
    }
}
