//Assets\Vurbiri\Runtime\Types\Enum\EnumFlags.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public struct EnumFlags<T> : IReadOnlyList<bool>, IEquatable<EnumFlags<T>>, IEquatable<int> where T : Enum
	{
        private static readonly int s_maskValue;

        public static readonly EnumFlags<T> None = new(false);
        public static readonly EnumFlags<T> Fill = new(true);

        static EnumFlags()
        {
            var values = Enum<T>.Values;
            int count = Enum<T>.count;

            s_maskValue = ~(-1 << count);

#if UNITY_EDITOR
            Throw.IfGreater(count, 32);
            int value, oldValue = -1;
            for (int i = 0; i < count; i++)
            {
                value = values[i].ToInt();
                if ((value - oldValue) != 1)
                    Errors.Message("The wrong type. Enum values must be equal to or greater than zero and in sequence.");
                oldValue = value;
            }
#endif
        }

        [SerializeField] private int _value;

        public readonly int Count => Enum<T>.count;

        public readonly bool this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((_value >> i) & 1) > 0;
        }

        public readonly bool this[T e] 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((_value >> e.ToInt()) & 1) > 0;
        }

        #region Constructors
        public EnumFlags(int value)
		{
            Throw.IfOutOfRange(value, 0, Enum<T>.count);
            _value = 1 << value;
        }
        public EnumFlags(T value)
        {
            _value = 1 << value.ToInt();
        }
        public EnumFlags(bool all)
        {
            if (all) _value = s_maskValue; else _value = 0;
        }

        private EnumFlags(int value, int i, bool operation)
        {
            Throw.IfOutOfRange(i, 0, Enum<T>.count);
            if (operation) value |= 1 << i; else value ^= 1 << i;
            _value = value;
        }
        private EnumFlags(int value, T e, bool operation)
        {
            if (operation) value |= 1 << e.ToInt(); else value ^= 1 << e.ToInt();
            _value = value;
        }
        #endregion

        public readonly bool Equals(EnumFlags<T> other) => (_value & s_maskValue) == (other._value & s_maskValue);
        public readonly bool Equals(int i) => ((_value >> i) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is EnumFlags<T> flags) return Equals(flags);
            if (obj is int i) return Equals(i);
            if (obj is T e) return Equals(e.ToInt());

            return false;
        }
        public override readonly int GetHashCode() => _value.GetHashCode();

        public static implicit operator EnumFlags<T>(int value) => new(value);
        public static implicit operator EnumFlags<T>(T value) => new(value);
        public static implicit operator EnumFlags<T>(bool all) => new(all);

        public static bool operator ==(EnumFlags<T> a, EnumFlags<T> b) => (a._value & s_maskValue) == (b._value & s_maskValue);
        public static bool operator !=(EnumFlags<T> a, EnumFlags<T> b) => (a._value & s_maskValue) != (b._value & s_maskValue);

        public static bool operator ==(EnumFlags<T> flags, int i) => ((flags._value >> i) & 1) > 0;
        public static bool operator !=(EnumFlags<T> flags, int i) => ((flags._value >> i) & 1) == 0;

        public static bool operator ==(int i, EnumFlags<T> flags) => ((flags._value >> i) & 1) > 0;
        public static bool operator !=(int i, EnumFlags<T> flags) => ((flags._value >> i) & 1) == 0;

        public static bool operator ==(EnumFlags<T> flags, T e) => ((flags._value >> e.ToInt()) & 1) > 0;
        public static bool operator !=(EnumFlags<T> flags, T e) => ((flags._value >> e.ToInt()) & 1) == 0;

        public static bool operator ==(T e, EnumFlags<T> flags) => ((flags._value >> e.ToInt()) & 1) > 0;
        public static bool operator !=(T e, EnumFlags<T> flags) => ((flags._value >> e.ToInt()) & 1) == 0;

        public static EnumFlags<T> operator |(EnumFlags<T> flags, int i) => new(flags._value, i, true);
        public static EnumFlags<T> operator |(int i, EnumFlags<T> flags) => new(flags._value, i, true);

        public static EnumFlags<T> operator ^(EnumFlags<T> flags, int i) => new(flags._value, i, false);
        public static EnumFlags<T> operator ^(int i, EnumFlags<T> flags) => new(flags._value, i, false);

        public static EnumFlags<T> operator |(EnumFlags<T> flags, T e) => new(flags._value, e, true);
        public static EnumFlags<T> operator |(T e, EnumFlags<T> flags) => new(flags._value, e, true);

        public static EnumFlags<T> operator ^(EnumFlags<T> flags, T e) => new(flags._value, e, false);
        public static EnumFlags<T> operator ^(T e, EnumFlags<T> flags) => new(flags._value, e, false);

        public readonly IEnumerator<bool> GetEnumerator()
        {
			for (int i = 0; i < Enum<T>.count; i++)
				yield return this[i];
        }
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
