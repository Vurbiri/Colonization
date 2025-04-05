//Assets\Vurbiri\Runtime\Types\EnumFlags.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public struct EnumFlags<T> : IReadOnlyList<bool>, IEquatable<EnumFlags<T>>, IEquatable<int> where T : Enum
	{
        private const int MAX_COUNT = 32;
        private static readonly int count, maskValue;
        private static readonly string format;

        static EnumFlags()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            count = values.Length;
            Throw.IfGreater(count, MAX_COUNT);

            int value, oldValue = -1;
            for (int i = 0; i < count; i++)
            {
                value = values[i].ToInt();
                if ((value - oldValue) != 1)
                    Errors.Message("The wrong type. Values must be equal to or greater than zero and in sequence.");
                oldValue = value;
            }

            maskValue = ~(-1 << count);
            format = $"x{Mathf.FloorToInt(count * 0.25f)}";
        }

        [SerializeField] private int _value;

        public static readonly EnumFlags<T> Empty = new(false);

        public readonly int Count => count;

        public readonly bool this[int i] => ((_value >> i) & 1) > 0;
        public readonly bool this[T e] => ((_value >> e.ToInt()) & 1) > 0;

        #region Constructors
        public EnumFlags(int value)
		{
            Throw.IfOutOfRange(value, 0, count);
            _value = 1 << value;
        }
        public EnumFlags(T value)
        {
            _value = 1 << value.ToInt();
        }
        public EnumFlags(bool all)
        {
            if (all) _value = maskValue; else _value = 0;
        }

        private EnumFlags(int value, int i, bool operation)
        {
            Throw.IfOutOfRange(i, 0, count);
            if (operation) value |= 1 << i; else value ^= 1 << i;
            _value = value;
        }
        private EnumFlags(int value, T e, bool operation)
        {
            if (operation) value |= 1 << e.ToInt(); else value ^= 1 << e.ToInt();
            _value = value;
        }
        #endregion

        public void Fill() => _value = maskValue;
        public void Clear() => _value = 0;

        public override readonly string ToString() => "0x".Concat(_value.ToString(format));

        public readonly bool Equals(EnumFlags<T> other) => (_value & maskValue) == (other._value & maskValue);
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

        public static bool operator ==(EnumFlags<T> a, EnumFlags<T> b) => (a._value & maskValue) == (b._value & maskValue);
        public static bool operator !=(EnumFlags<T> a, EnumFlags<T> b) => (a._value & maskValue) != (b._value & maskValue);

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
			for (int i = 0; i < count; i++)
				yield return this[i];
        }
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
