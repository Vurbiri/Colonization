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
		[SerializeField] private int _value;
		[SerializeField] private int _count;

        public readonly int Count => _count;

        public bool this[int i]
		{
            readonly get
            {
                Errors.ThrowIfOutOfRange(i, _count);
                return ((_value >> i) & 1) > 0;
            }
			set
			{
                Errors.ThrowIfOutOfRange(i, _count);
                if (value) _value |= 1 << i;
				else _value ^= 1 << i;//_value &= ~(1 << i);
            }
		}
        public bool this[T e]
        {
            readonly get => this[e.ToInt()];
            set => this[e.ToInt()] = value;
        }

        public EnumFlags(int value)
		{
			_value = 1 << value; _count = Enum<T>.Count;
        }
        public EnumFlags(T value)
        {
            _value = 1 << value.ToInt(); _count = Enum<T>.Count;
        }
        public EnumFlags(bool all)
        {
            if (all) _value = -1; else _value = 0;
            _count = Enum<T>.Count;
        }

        private EnumFlags(int value, int count)
        {
            _value = value; _count = count;
        }

        public void Add(int i) => _value |= 1 << i;
        public void Add(T e) => _value |= 1 << e.ToInt();

        public void Remove(int i) => _value ^= 1 << i;
        public void Remove(T e) => _value ^= 1 << e.ToInt();

		public void Fill() => _value = -1;
		public void Clear() => _value = 0;

        public static implicit operator EnumFlags<T>(int value) => new(value);
        public static implicit operator EnumFlags<T>(T value) => new(value);

        public readonly bool Equals(EnumFlags<T> other) => _value == other._value;
        public readonly bool Equals(int i) => ((_value >> i) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;
            
            if (obj is EnumFlags<T> flags) return _value == flags._value;
            if (obj is int i) return ((_value >> i) & 1) > 0;
            if (obj is T e) return ((_value >> e.ToInt()) & 1) > 0;

            return false;
        }
        public override readonly int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(EnumFlags<T> a, EnumFlags<T> b) => a._value == b._value;
        public static bool operator !=(EnumFlags<T> a, EnumFlags<T> b) => a._value != b._value;

        public static bool operator ==(EnumFlags<T> flags, int i) => ((flags._value >> i) & 1) > 0;
        public static bool operator !=(EnumFlags<T> flags, int i) => ((flags._value >> i) & 1) == 0;

        public static bool operator ==(int i, EnumFlags<T> flags) => ((flags._value >> i) & 1) > 0;
        public static bool operator !=(int i, EnumFlags<T> flags) => ((flags._value >> i) & 1) == 0;

        public static bool operator ==(EnumFlags<T> flags, T e) => ((flags._value >> e.ToInt()) & 1) > 0;
        public static bool operator !=(EnumFlags<T> flags, T e) => ((flags._value >> e.ToInt()) & 1) == 0;

        public static bool operator ==(T e, EnumFlags<T> flags) => ((flags._value >> e.ToInt()) & 1) > 0;
        public static bool operator !=(T e, EnumFlags<T> flags) => ((flags._value >> e.ToInt()) & 1) == 0;

        public static EnumFlags<T> operator |(EnumFlags<T> flags, int i) => new(flags._value |= 1 << i, flags._count);
        public static EnumFlags<T> operator |(int i, EnumFlags<T> flags) => new(flags._value |= 1 << i, flags._count);

        public static EnumFlags<T> operator ^(EnumFlags<T> flags, int i) => new(flags._value ^= 1 << i, flags._count);
        public static EnumFlags<T> operator ^(int i, EnumFlags<T> flags) => new(flags._value ^= 1 << i, flags._count);

        public static EnumFlags<T> operator |(EnumFlags<T> flags, T e) => new(flags._value |= 1 << e.ToInt(), flags._count);
        public static EnumFlags<T> operator |(T e, EnumFlags<T> flags) => new(flags._value |= 1 << e.ToInt(), flags._count);

        public static EnumFlags<T> operator ^(EnumFlags<T> flags, T e) => new(flags._value ^= 1 << e.ToInt(), flags._count);
        public static EnumFlags<T> operator ^(T e, EnumFlags<T> flags) => new(flags._value ^= 1 << e.ToInt(), flags._count);
                
        public readonly IEnumerator<bool> GetEnumerator()
        {
			for (int i = 0; i < _count; i++)
				yield return this[i];
        }
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
