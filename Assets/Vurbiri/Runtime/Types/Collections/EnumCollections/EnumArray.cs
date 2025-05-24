using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public partial class EnumArray<TType, TValue> : IReadOnlyList<TValue> where TType : Enum
    {
        private static readonly int s_count;

        static EnumArray()
        {
            s_count = 0;
            var values = Enum<TType>.Values;
            int max = values.Length;
            for (int i = 0, j = 0, value; i < max; i++)
            {
                value = values[i].GetHashCode();
                if (value >= 0)
                {
                    if (value != j++)
                        throw new($"Type '{typeof(TType).Name}' is not supported by EnumArray<,>");

                    s_count++;
                }
            }
        }

        [SerializeField] protected TValue[] _values;

        public int Count => s_count;

        public virtual TValue this[TType type] { get => _values[type.GetHashCode()]; set => _values[type.GetHashCode()] = value; }
        public virtual TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        public IReadOnlyList<TValue> Values => _values;

        public EnumArray()
        {
            _values = new TValue[s_count];
        }

        public EnumArray(TValue defaultValue) : this()
        {
            for (int i = 0; i < s_count; i++)
                _values[i] = defaultValue;
        }

        public EnumArray(IReadOnlyList<TValue> collection) : this()
        {
            int length = s_count <= collection.Count ? s_count : collection.Count;
            for (int i = 0; i < length; i++)
                _values[i] = collection[i];
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < s_count; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator EnumArray<TType, TValue>(TValue[] values) => new(values);
    }
}
