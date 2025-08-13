using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class ReadOnlyIdArray<TId, TValue> : IReadOnlyList<TValue> where TId : IdType<TId>
    {
        [SerializeField] protected TValue[] _values = new TValue[IdType<TId>.Count];

        public int Count => IdType<TId>.Count;

        public ReadOnlyCollection<TValue> ReadOnlyValues => new(_values);

        public TValue this[Id<TId> id] => _values[id.Value];
        public TValue this[int index] => _values[index];

        protected ReadOnlyIdArray() {}
        public ReadOnlyIdArray(TValue defaultValue)
        {
            for (int i = 0; i < IdType<TId>.Count; i++)
                _values[i] = defaultValue;
        }
        public ReadOnlyIdArray(Func<TValue> factory)
        {
            for (int i = 0; i < IdType<TId>.Count; i++)
                _values[i] = factory();
        }
        [JsonConstructor]
        public ReadOnlyIdArray(IReadOnlyList<TValue> list)
        {
            int count = Mathf.Min(IdType<TId>.Count, list.Count);
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }

        public IEnumerator<TValue> GetEnumerator() => new ArrayEnumerator<TValue>(_values);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<TValue>(_values);

        public static implicit operator ReadOnlyIdArray<TId, TValue>(TValue[] value) => new(value);
        public static implicit operator ReadOnlyIdArray<TId, TValue>(List<TValue> value) => new(value);
    }

    [Serializable, JsonArray]
    public class IdArray<TId, TValue> : ReadOnlyIdArray<TId, TValue> where TId : IdType<TId>
    {
        public TValue[] Values => _values;

        public new TValue this[Id<TId> id] { get => _values[id.Value]; set => _values[id.Value] = value; }
        public new TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        [JsonConstructor]
        public IdArray(IReadOnlyList<TValue> list) : base(list) { }
        public IdArray() { }
        public IdArray(TValue defaultValue) : base(defaultValue) { }
        public IdArray(Func<TValue> factory) : base(factory) { }

        public static implicit operator IdArray<TId, TValue>(TValue[] value) => new(value);
        public static implicit operator IdArray<TId, TValue>(List<TValue> value) => new(value);
    }
}
