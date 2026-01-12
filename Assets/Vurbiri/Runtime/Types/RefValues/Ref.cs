using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[Serializable]
	public abstract class Ref<T> : IEquatable<T>, IEquatable<Ref<T>>, IComparable<T>, IComparable<Ref<T>>
	where T : struct, IEquatable<T>, IComparable<T>
	{
		[SerializeField] protected T _value;

		public T Value { [Impl(256)] get => _value; }

		#region Equals
		[Impl(256)] public bool Equals(T other) => _value.Equals(other);
		[Impl(256)] public bool Equals(Ref<T> other) => other is not null && _value.Equals(other._value);
		sealed public override bool Equals(object other) => other is not null && ((other is T t && _value.Equals(t)) || (other is Ref<T> r && _value.Equals(r._value)));
		#endregion

		#region CompareTo
		[Impl(256)] public int CompareTo(T other) => _value.CompareTo(other);
		[Impl(256)] public int CompareTo(Ref<T> other) => _value.CompareTo(other._value);
		#endregion

		[Impl(256)] sealed public override int GetHashCode() => _value.GetHashCode();
		[Impl(256)] public override string ToString() => _value.ToString();


		[Impl(256)] public static implicit operator T(Ref<T> self) => self._value;

		#region Comparison operator
		[Impl(256)] public static bool operator ==(Ref<T> a, Ref<T> b) => ReferenceEquals(a, b) || ((a is not null & b is not null) && a._value.Equals(b._value));
		[Impl(256)] public static bool operator !=(Ref<T> a, Ref<T> b) => !(a == b);

		[Impl(256)] public static bool operator <(Ref<T> a, Ref<T> b) => a._value.CompareTo(b._value) < 0;
		[Impl(256)] public static bool operator <=(Ref<T> a, Ref<T> b) => a._value.CompareTo(b._value) <= 0;
		[Impl(256)] public static bool operator >(Ref<T> a, Ref<T> b) => a._value.CompareTo(b._value) > 0;
		[Impl(256)] public static bool operator >=(Ref<T> a, Ref<T> b) => a._value.CompareTo(b._value) >= 0;


		[Impl(256)] public static bool operator ==(Ref<T> r, T t) => r._value.Equals(t);
		[Impl(256)] public static bool operator !=(Ref<T> r, T t) => !r._value.Equals(t);

		[Impl(256)] public static bool operator <(Ref<T> r, T t) => r._value.CompareTo(t) < 0;
		[Impl(256)] public static bool operator <=(Ref<T> r, T t) => r._value.CompareTo(t) <= 0;
		[Impl(256)] public static bool operator >(Ref<T> r, T t) => r._value.CompareTo(t) > 0;
		[Impl(256)] public static bool operator >=(Ref<T> r, T t) => r._value.CompareTo(t) >= 0;

		[Impl(256)] public static bool operator ==(T t, Ref<T> r) => t.Equals(r._value);
		[Impl(256)] public static bool operator !=(T t, Ref<T> r) => !t.Equals(r._value);

		[Impl(256)] public static bool operator <(T t, Ref<T> r) => t.CompareTo(r._value) < 0;
		[Impl(256)] public static bool operator <=(T t, Ref<T> r) => t.CompareTo(r._value) <= 0;
		[Impl(256)] public static bool operator >(T t, Ref<T> r) => t.CompareTo(r._value) > 0;
		[Impl(256)] public static bool operator >=(T t, Ref<T> r) => t.CompareTo(r._value) >= 0;
		#endregion
	}
}
