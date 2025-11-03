using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
    [Serializable]
    public abstract class ReactiveValue<T> : IReactiveValue<T>, IEquatable<T>, IEquatable<ReactiveValue<T>>, IComparable<T>, IComparable<ReactiveValue<T>>
    where T : struct, IEquatable<T>, IComparable<T>
    {
        [SerializeField] protected T _value;

        protected readonly VAction<T> _changeEvent = new();

        public T Value { [Impl(256)] get => _value; }

        [Impl(256)] public Subscription Subscribe(Action<T> action, bool instantGetValue = true) => _changeEvent.Add(action, instantGetValue, _value);
        [Impl(256)] public void Unsubscribe(Action<T> action) => _changeEvent.Remove(action);

        public bool Equals(T other) => _value.Equals(other);
        public bool Equals(ReactiveValue<T> other) => other is not null && _value.Equals(other._value);
        sealed public override bool Equals(object other) => other is not null && ((other is T t && _value.Equals(t)) || (other is ReactiveValue<T> r && _value.Equals(r._value)));

        public int CompareTo(T other) => _value.CompareTo(other);
        public int CompareTo(ReactiveValue<T> other) => _value.CompareTo(other._value);

        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();

        [Impl(256)] public static implicit operator T(ReactiveValue<T> self) => self._value;

        #region Comparison operator
        public static bool operator ==(ReactiveValue<T> a, ReactiveValue<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b._value));
        public static bool operator !=(ReactiveValue<T> a, ReactiveValue<T> b) => !(a == b);

        public static bool operator <(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) < 0;
        public static bool operator <=(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) <= 0;
        public static bool operator >(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) > 0;
        public static bool operator >=(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) >= 0;


        public static bool operator ==(ReactiveValue<T> r, T t) => r._value.Equals(t);
        public static bool operator !=(ReactiveValue<T> r, T t) => !r._value.Equals(t);

        public static bool operator <(ReactiveValue<T> r, T t) => r._value.CompareTo(t) < 0;
        public static bool operator <=(ReactiveValue<T> r, T t) => r._value.CompareTo(t) <= 0;
        public static bool operator >(ReactiveValue<T> r, T t) => r._value.CompareTo(t) > 0;
        public static bool operator >=(ReactiveValue<T> r, T t) => r._value.CompareTo(t) >= 0;

        public static bool operator ==(T t, ReactiveValue<T> r) => t.Equals(r._value);
        public static bool operator !=(T t, ReactiveValue<T> r) => !t.Equals(r._value);

        public static bool operator <(T t, ReactiveValue<T> r) => t.CompareTo(r._value) < 0;
        public static bool operator <=(T t, ReactiveValue<T> r) => t.CompareTo(r._value) <= 0;
        public static bool operator >(T t, ReactiveValue<T> r) => t.CompareTo(r._value) > 0;
        public static bool operator >=(T t, ReactiveValue<T> r) => t.CompareTo(r._value) >= 0;

        public static bool operator ==(ReactiveValue<T> a, IReactiveValue<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b.Value));
        public static bool operator !=(ReactiveValue<T> a, IReactiveValue<T> b) => !(a == b);

        public static bool operator <(ReactiveValue<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) < 0;
        public static bool operator <=(ReactiveValue<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) <= 0;
        public static bool operator >(ReactiveValue<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) > 0;
        public static bool operator >=(ReactiveValue<T> a, IReactiveValue<T> b) => a._value.CompareTo(b.Value) >= 0;

        public static bool operator ==(IReactiveValue<T> a, ReactiveValue<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a.Value.Equals(b._value));
        public static bool operator !=(IReactiveValue<T> a, ReactiveValue<T> b) => !(a == b);

        public static bool operator <(IReactiveValue<T> a, ReactiveValue<T> b) => a.Value.CompareTo(b._value) < 0;
        public static bool operator <=(IReactiveValue<T> a, ReactiveValue<T> b) => a.Value.CompareTo(b._value) <= 0;
        public static bool operator >(IReactiveValue<T> a, ReactiveValue<T> b) => a.Value.CompareTo(b._value) > 0;
        public static bool operator >=(IReactiveValue<T> a, ReactiveValue<T> b) => a.Value.CompareTo(b._value) >= 0;
        #endregion
    }

}
