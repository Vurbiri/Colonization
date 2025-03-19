//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\IdArray.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class IdArray<TId, TValue> : IReadOnlyList<TValue> where TId : IdType<TId>
    {
        [SerializeField] protected TValue[] _values;
        protected int _count;

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _count;
        }

        public virtual TValue this[Id<TId> id] { get => _values[id.Value]; set => _values[id.Value] = value; }
        public virtual TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        public IdArray()
        {
            _count = IdType<TId>.Count;
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

        public IdArray(IReadOnlyList<TValue> list) : this() 
        {
            int count = _count <= list.Count ? _count : list.Count;
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator IdArray<TId, TValue>(TValue[] value) => new(value);
    }
}
