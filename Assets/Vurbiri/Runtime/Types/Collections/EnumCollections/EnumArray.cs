//Assets\Vurbiri\Runtime\Types\Collections\EnumCollections\EnumArray.cs
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
        public static readonly int count;

        static EnumArray()
        {
            count = 0;
            TType[] values = Enum<TType>.Values;
            for (int i = values.Length - 1; i >= 0; i--)
            {
                if (values[i].ToInt() >= 0)
                    count++;
            }
        }

        [SerializeField] protected TValue[] _values;

        public int Count => count;

        public virtual TValue this[TType type] { get => _values[type.ToInt()]; set => _values[type.ToInt()] = value; }
        public virtual TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        public IReadOnlyList<TValue> Values => _values;

        public EnumArray()
        {
            _values = new TValue[count];
        }

        public EnumArray(TValue defaultValue) : this()
        {
            for (int i = 0; i < count; i++)
                _values[i] = defaultValue;
        }

        public EnumArray(IReadOnlyList<TValue> collection) : this()
        {
            int length = count <= collection.Count ? count : collection.Count;
            for (int i = 0; i < length; i++)
                _values[i] = collection[i];
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator EnumArray<TType, TValue>(TValue[] values) => new(values);
    }
}
