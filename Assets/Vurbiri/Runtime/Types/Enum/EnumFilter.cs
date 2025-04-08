//Assets\Vurbiri\Runtime\Types\Enum\EnumFilter.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public struct EnumFilter<T> : IEquatable<EnumFilter<T>>, IEquatable<T> where T : Enum
    {
        private const int MAX_COUNT = 32;
        private static readonly int count, maskValue;
        private static readonly string format;
        private static readonly Dictionary<T, int> ids;

        public static readonly EnumFilter<T> Empty = new(false);

        static EnumFilter()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            count = values.Length;

            Throw.IfGreater(count, MAX_COUNT);

            maskValue = ~(-1 << count);
            format = $"x{Mathf.FloorToInt(0.25f * count)}";
            ids = new(count);

            for (int i = 0; i < count; i++)
                ids.Add(values[i], i);
        }

        [SerializeField] private int _value;
        
        public readonly int Count => count;

        public readonly bool this[T e] => ((_value >> ids[e]) & 1) > 0;

        #region Constructors
        public EnumFilter(T e)
        {
            _value = 1 << ids[e];
        }
        public EnumFilter(bool all)
        {
            if (all) _value = maskValue; else _value = 0;
        }
        private EnumFilter(int value)
        {
            _value = value & maskValue;
        }
        #endregion

        public override readonly string ToString() => "0x".Concat(_value.ToString(format));

        public readonly bool Equals(EnumFilter<T> other) => (_value & maskValue) == (other._value & maskValue);
        public readonly bool Equals(T e) => ((_value >> ids[e]) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is EnumFilter<T> filter) return (_value & maskValue) == (filter._value & maskValue);
            if (obj is T e) return ((_value >> ids[e]) & 1) > 0;

            return false;
        }
        public override readonly int GetHashCode() => _value.GetHashCode();

        public static implicit operator EnumFilter<T>(T value) => new(value);
        public static implicit operator EnumFilter<T>(bool all) => new(all);

        public static bool operator ==(EnumFilter<T> a, EnumFilter<T> b) => (a._value & maskValue) == (b._value & maskValue);
        public static bool operator !=(EnumFilter<T> a, EnumFilter<T> b) => (a._value & maskValue) != (b._value & maskValue);

        public static bool operator ==(EnumFilter<T> filter, T e) => ((filter._value >> ids[e]) & 1) > 0;
        public static bool operator !=(EnumFilter<T> filter, T e) => ((filter._value >> ids[e]) & 1) == 0;

        public static bool operator ==(T e, EnumFilter<T> filter) => ((filter._value >> ids[e]) & 1) > 0;
        public static bool operator !=(T e, EnumFilter<T> filter) => ((filter._value >> ids[e]) & 1) == 0;

        public static EnumFilter<T> operator |(EnumFilter<T> filter, T e) => new(filter._value |= 1 << ids[e]);
        public static EnumFilter<T> operator |(T e, EnumFilter<T> filter) => new(filter._value |= 1 << ids[e]);

        public static EnumFilter<T> operator ^(EnumFilter<T> filter, T e) => new(filter._value ^= 1 << ids[e]);
        public static EnumFilter<T> operator ^(T e, EnumFilter<T> filter) => new(filter._value ^= 1 << ids[e]);
    }
}
