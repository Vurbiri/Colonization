using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Reactive
{
	public abstract class ReactiveValue<T> : Reactive<T>, IReactiveValue<T>, IEquatable<ReactiveValue<T>>, IEquatable<T>, IComparable<ReactiveValue<T>>, IComparable<T>
		where T : struct, IEquatable<T>, IComparable<T>
	{
		public new T Value
		{
			[Impl(256)] get => _value;
			[Impl(256)] set { if (!_value.Equals(value)) _onChange.Invoke(_value = value); }
		}

		public T SilentValue
		{
			[Impl(256)] get => _value;
			[Impl(256)] set => _value = value;
		}

		[Impl(256)] public void UnsubscribeAll() => _onChange.Clear();
		[Impl(256)] public void Signal() => _onChange.Invoke(_value);

        [Impl(256)] sealed public override int GetHashCode() => _value.GetHashCode();
        
        #region Equals
        [Impl(256)] public bool Equals(ReactiveValue<T> other) => other is not null && _value.Equals(other._value);
        [Impl(256)] public bool Equals(T value) => _value.Equals(value);
        [Impl(256)] public override bool Equals(object other) => other is not null && ((other is T v && _value.Equals(v)) || (other is ReactiveValue<T> r && _value.Equals(r._value)));
        #endregion

        #region CompareTo
        [Impl(256)] public int CompareTo(ReactiveValue<T> other) => _value.CompareTo(other._value);
        [Impl(256)] public int CompareTo(T value) => _value.CompareTo(value);
        #endregion

        #region Comparison operator
        [Impl(256)] public static bool operator ==(ReactiveValue<T> a, ReactiveValue<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b._value));
        [Impl(256)] public static bool operator !=(ReactiveValue<T> a, ReactiveValue<T> b) => !(a == b);

        [Impl(256)] public static bool operator <(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) < 0;
        [Impl(256)] public static bool operator <=(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) <= 0;
        [Impl(256)] public static bool operator >(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) > 0;
        [Impl(256)] public static bool operator >=(ReactiveValue<T> a, ReactiveValue<T> b) => a._value.CompareTo(b._value) >= 0;

        [Impl(256)] public static bool operator ==(ReactiveValue<T> r, T t) => r._value.Equals(t);
        [Impl(256)] public static bool operator !=(ReactiveValue<T> r, T t) => !r._value.Equals(t);

        [Impl(256)] public static bool operator <(ReactiveValue<T> r, T t) => r._value.CompareTo(t) < 0;
        [Impl(256)] public static bool operator <=(ReactiveValue<T> r, T t) => r._value.CompareTo(t) <= 0;
        [Impl(256)] public static bool operator >(ReactiveValue<T> r, T t) => r._value.CompareTo(t) > 0;
        [Impl(256)] public static bool operator >=(ReactiveValue<T> r, T t) => r._value.CompareTo(t) >= 0;

        [Impl(256)] public static bool operator ==(T t, ReactiveValue<T> r) => t.Equals(r._value);
        [Impl(256)] public static bool operator !=(T t, ReactiveValue<T> r) => !t.Equals(r._value);

        [Impl(256)] public static bool operator <(T t, ReactiveValue<T> r) => t.CompareTo(r._value) < 0;
        [Impl(256)] public static bool operator <=(T t, ReactiveValue<T> r) => t.CompareTo(r._value) <= 0;
        [Impl(256)] public static bool operator >(T t, ReactiveValue<T> r) => t.CompareTo(r._value) > 0;
        [Impl(256)] public static bool operator >=(T t, ReactiveValue<T> r) => t.CompareTo(r._value) >= 0;
        #endregion
    }
}
