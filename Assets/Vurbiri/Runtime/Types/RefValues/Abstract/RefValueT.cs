using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public abstract class RefValue<T> : Ref<T>, IEquatable<RefValue<T>>, IEquatable<T>, IComparable<RefValue<T>>, IComparable<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        public new T Value { [Impl(256)] get => _value; [Impl(256)] set => _value = value; }

        [Impl(256)] public override int GetHashCode() => _value.GetHashCode();

        #region Equals
        [Impl(256)] public bool Equals(RefValue<T> other) => other is not null && _value.Equals(other._value);
        [Impl(256)] public bool Equals(T value) => _value.Equals(value);
        [Impl(256)] public override bool Equals(object other) => other is not null && ((other is T v && _value.Equals(v)) || (other is RefValue<T> r && _value.Equals(r._value)));
        #endregion

        #region CompareTo
        [Impl(256)] public int CompareTo(RefValue<T> other) => _value.CompareTo(other._value);
        [Impl(256)] public int CompareTo(T value) => _value.CompareTo(value);
        #endregion

        #region Comparison operator
        [Impl(256)] public static bool operator ==(RefValue<T> a, RefValue<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b._value));
        [Impl(256)] public static bool operator !=(RefValue<T> a, RefValue<T> b) => !(a == b);

        [Impl(256)] public static bool operator <(RefValue<T> a, RefValue<T> b) => a._value.CompareTo(b._value) < 0;
        [Impl(256)] public static bool operator <=(RefValue<T> a, RefValue<T> b) => a._value.CompareTo(b._value) <= 0;
        [Impl(256)] public static bool operator >(RefValue<T> a, RefValue<T> b) => a._value.CompareTo(b._value) > 0;
        [Impl(256)] public static bool operator >=(RefValue<T> a, RefValue<T> b) => a._value.CompareTo(b._value) >= 0;

        [Impl(256)] public static bool operator ==(RefValue<T> r, T t) => r._value.Equals(t);
        [Impl(256)] public static bool operator !=(RefValue<T> r, T t) => !r._value.Equals(t);

        [Impl(256)] public static bool operator <(RefValue<T> r, T t) => r._value.CompareTo(t) < 0;
        [Impl(256)] public static bool operator <=(RefValue<T> r, T t) => r._value.CompareTo(t) <= 0;
        [Impl(256)] public static bool operator >(RefValue<T> r, T t) => r._value.CompareTo(t) > 0;
        [Impl(256)] public static bool operator >=(RefValue<T> r, T t) => r._value.CompareTo(t) >= 0;

        [Impl(256)] public static bool operator ==(T t, RefValue<T> r) => t.Equals(r._value);
        [Impl(256)] public static bool operator !=(T t, RefValue<T> r) => !t.Equals(r._value);

        [Impl(256)] public static bool operator <(T t, RefValue<T> r) => t.CompareTo(r._value) < 0;
        [Impl(256)] public static bool operator <=(T t, RefValue<T> r) => t.CompareTo(r._value) <= 0;
        [Impl(256)] public static bool operator >(T t, RefValue<T> r) => t.CompareTo(r._value) > 0;
        [Impl(256)] public static bool operator >=(T t, RefValue<T> r) => t.CompareTo(r._value) >= 0;
        #endregion
    }
}
