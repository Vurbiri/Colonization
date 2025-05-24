using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Reactive
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public abstract class ARType<T> : IReactiveValue<T>, IEquatable<T>, IEquatable<ARType<T>>, IComparable<T>, IComparable<ARType<T>> 
    where T : struct, IEquatable<T>, IComparable<T>
    {
        [SerializeField, JsonProperty("value")]
        protected T _value;
        protected readonly Subscription<T> _subscriber = new();

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value;
            set { if (!_value.Equals(value)) _subscriber.Invoke(_value = value); }
        }

        public T SilentValue { get => _value; set => _value = value; }

        public ARType(T value) => _value = value;

        public Unsubscription Subscribe(Action<T> action, bool instantGetValue = true) => _subscriber.Add(action, instantGetValue, _value);
        public void Signal() => _subscriber.Invoke(_value);

        public bool Equals(T other) => _value.Equals(other);
        public bool Equals(ARType<T> other) => other is not null && _value.Equals(other._value);

        public int CompareTo(T other) => _value.CompareTo(other);
        public int CompareTo(ARType<T> other) => _value.CompareTo(other._value);

        sealed public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other is T tValue) return _value.Equals(tValue);
            if (other is ARType<T> rValue) return _value.Equals(rValue._value);
            return false;
        }

        public override int GetHashCode() => _value.GetHashCode();

        #region Comparison operator
        public static bool operator ==(ARType<T> a, ARType<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b._value));
        public static bool operator !=(ARType<T> a, ARType<T> b) => !(a == b);

        public static bool operator <(ARType<T> a, ARType<T> b) => a._value.CompareTo(b._value) < 0;
        public static bool operator <=(ARType<T> a, ARType<T> b) => a._value.CompareTo(b._value) <= 0;
        public static bool operator >(ARType<T> a, ARType<T> b) => a._value.CompareTo(b._value) > 0;
        public static bool operator >=(ARType<T> a, ARType<T> b) => a._value.CompareTo(b._value) >= 0;

        public static bool operator ==(ARType<T> a, IReactiveValue<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b.Value));
        public static bool operator !=(ARType<T> a, IReactiveValue<T> b) => !(a == b);

        public static bool operator <(ARType<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) < 0;
        public static bool operator <=(ARType<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) <= 0;
        public static bool operator >(ARType<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) > 0;
        public static bool operator >=(ARType<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) >= 0;

        public static bool operator ==(IReactiveValue<T> a, ARType<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a.Value.Equals(b._value));
        public static bool operator !=(IReactiveValue<T> a, ARType<T> b) => !(a == b);

        public static bool operator <(IReactiveValue<T> a, ARType<T> b) => a.Value.CompareTo(b._value) < 0;
        public static bool operator <=(IReactiveValue<T> a, ARType<T> b) => a.Value.CompareTo(b._value) <= 0;
        public static bool operator >(IReactiveValue<T> a, ARType<T> b) => a.Value.CompareTo(b._value) > 0;
        public static bool operator >=(IReactiveValue<T> a, ARType<T> b) => a.Value.CompareTo(b._value) >= 0;

        public static bool operator ==(ARType<T> r, T t) => r._value.Equals(t);
        public static bool operator !=(ARType<T> r, T t) => !r._value.Equals(t);

        public static bool operator <(ARType<T> r, T t) => r._value.CompareTo(t) < 0;
        public static bool operator <=(ARType<T> r, T t) => r._value.CompareTo(t) <= 0;
        public static bool operator >(ARType<T> r, T t) => r._value.CompareTo(t) > 0;
        public static bool operator >=(ARType<T> r, T t) => r._value.CompareTo(t) >= 0;

        public static bool operator ==(T t, ARType<T> r) => t.Equals(r._value);
        public static bool operator !=(T t, ARType<T> r) => !t.Equals(r._value);

        public static bool operator <(T t, ARType<T> r) => t.CompareTo(r._value) < 0;
        public static bool operator <=(T t, ARType<T> r) => t.CompareTo(r._value) <= 0;
        public static bool operator >(T t, ARType<T> r) => t.CompareTo(r._value) > 0;
        public static bool operator >=(T t, ARType<T> r) => t.CompareTo(r._value) >= 0;
        #endregion
    }
}
