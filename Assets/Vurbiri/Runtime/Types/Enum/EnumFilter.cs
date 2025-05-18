//Assets\Vurbiri\Runtime\Types\Enum\EnumFilter.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
	public struct EnumFilter<T> : IEquatable<EnumFilter<T>>, IEquatable<T>, IEnumerable<bool> where T : Enum
    {
        private static readonly int s_maskValue;
        private static readonly Dictionary<T, int> s_ids;

        public static readonly int Count;
        public static readonly EnumFilter<T> None = new(0);
        public static readonly EnumFilter<T> All = new(s_maskValue);
        public static IReadOnlyCollection<T> Values => s_ids.Keys;

        static EnumFilter()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            Count = values.Length;

            Throw.IfGreater(Count, 32);

            s_maskValue = ~(-1 << Count);
            s_ids = new(Count);

            for (int i = 0; i < Count; i++)
                s_ids.Add(values[i], i);
        }

        [SerializeField] private int _value;

        public readonly bool this[T e] => ((_value >> s_ids[e]) & 1) > 0;

        #region Constructors
        public EnumFilter(T e)
        {
            _value = 1 << s_ids[e];
        }
        public EnumFilter(bool all)
        {
            _value = all ? s_maskValue : 0;
        }
        private EnumFilter(int value)
        {
            _value = value & s_maskValue;
        }
        #endregion

        public readonly bool Equals(EnumFilter<T> other) => (_value & s_maskValue) == (other._value & s_maskValue);
        public readonly bool Equals(T e) => ((_value >> s_ids[e]) & 1) > 0;
        public override readonly bool Equals(object obj)
        {
            if (obj is null) return false;

            if (obj is EnumFilter<T> filter) return (_value & s_maskValue) == (filter._value & s_maskValue);
            if (obj is T e) return ((_value >> s_ids[e]) & 1) > 0;

            return false;
        }
        public override readonly int GetHashCode() => _value.GetHashCode();

        public readonly IEnumerator<bool> GetEnumerator()
        {
            for(int i = 0; i < Count; i++)
                yield return ((_value >> i) & 1) > 0;
        }
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator EnumFilter<T>(T value) => new(value);
        public static implicit operator EnumFilter<T>(bool all) => new(all);

        public static bool operator ==(EnumFilter<T> a, EnumFilter<T> b) => (a._value & s_maskValue) == (b._value & s_maskValue);
        public static bool operator !=(EnumFilter<T> a, EnumFilter<T> b) => (a._value & s_maskValue) != (b._value & s_maskValue);

        public static bool operator ==(EnumFilter<T> filter, T e) => ((filter._value >> s_ids[e]) & 1) > 0;
        public static bool operator !=(EnumFilter<T> filter, T e) => ((filter._value >> s_ids[e]) & 1) == 0;

        public static bool operator ==(T e, EnumFilter<T> filter) => ((filter._value >> s_ids[e]) & 1) > 0;
        public static bool operator !=(T e, EnumFilter<T> filter) => ((filter._value >> s_ids[e]) & 1) == 0;

        public static EnumFilter<T> operator |(EnumFilter<T> filter, T e) => new(filter._value |= 1 << s_ids[e]);
        public static EnumFilter<T> operator |(T e, EnumFilter<T> filter) => new(filter._value |= 1 << s_ids[e]);

        public static EnumFilter<T> operator ^(EnumFilter<T> filter, T e) => new(filter._value ^= 1 << s_ids[e]);
        public static EnumFilter<T> operator ^(T e, EnumFilter<T> filter) => new(filter._value ^= 1 << s_ids[e]);
    }
}
