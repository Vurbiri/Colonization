using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
#pragma warning disable 660, 661
	[Serializable]
	sealed public class RefFloat : RefValue<float>, IEquatable<RefFloat>, IComparable<RefFloat>
	{
		public new float Value { [Impl(256)] get => _value; [Impl(256)] set => _value = value; }

		[Impl(256)] public RefFloat() { }
		[Impl(256)] public RefFloat(float value) => _value = value;

		[Impl(256)] public bool Equals(RefFloat other) => other is not null && other._value == this._value;

		[Impl(256)] public int CompareTo(RefFloat other) => other is not null ? _value.CompareTo(other._value) : 1;

		//[Impl(256)] public static implicit operator RefFloat(float value) => new(value);
		[Impl(256)] public static implicit operator float(RefFloat value) => value._value;
		[Impl(256)] public static explicit operator int(RefFloat value) => (int)value._value;
	}
#pragma warning restore 660, 661
}
