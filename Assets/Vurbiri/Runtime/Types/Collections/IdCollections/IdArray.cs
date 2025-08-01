using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    sealed public class IdArray<TId, TValue> : IReadOnlyList<TValue> where TId : IdType<TId>
    {
        [SerializeField] private TValue[] _values;
        private readonly int _count = IdType<TId>.Count;

        public int Count => _count;

        public ReadOnlyCollection<TValue> Values => new(_values);

        public TValue this[Id<TId> id] { get => _values[id.Value]; set => _values[id.Value] = value; }
        public TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        public IdArray()
        {
            _values = new TValue[_count];
        }

        public IdArray(TValue defaultValue) : this()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = defaultValue;
        }

        public IdArray(Func<TValue> factory) : this()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = factory();
        }

        [JsonConstructor]
        public IdArray(IReadOnlyList<TValue> list) : this() 
        {
            int count = _count <= list.Count ? _count : list.Count;
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }

        public IEnumerator<TValue> GetEnumerator() => new ArrayEnumerator<TValue>(_values);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<TValue>(_values);

        public static implicit operator IdArray<TId, TValue>(TValue[] value) => new(value);
        public static implicit operator IdArray<TId, TValue>(List<TValue> value) => new(value);
    }
}
