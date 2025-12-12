using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public partial class ReadOnlyIdSet<TId, TValue> : IReadOnlyList<TValue> where TId : IdType<TId> where TValue : class, IValueId<TId>
    {
        [SerializeField] protected TValue[] _values = new TValue[IdType<TId>.Count];
        [SerializeField] protected int _count;

        protected readonly Version _version = new();

        public int Fullness { [Impl(256)] get => _count; }
        public int Count { [Impl(256)] get => IdType<TId>.Count; }
        public bool IsFull { [Impl(256)] get => _count == IdType<TId>.Count; }
        public bool IsNotFull { [Impl(256)] get => _count < IdType<TId>.Count; }

        public TValue this[int id]     { [Impl(256)] get => _values[id]; }
        public TValue this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; }

        public ReadOnlyIdSet() { }

        [JsonConstructor]
        public ReadOnlyIdSet(IReadOnlyList<TValue> list)
        {
            TValue value;
            for (int i = list.Count - 1; i >= 0; --i)
            {
                if ((value = list[i]) != null)
                {
                    _values[value.Id.Value] = value;
                    _count++;
                }
            }
        }
        public ReadOnlyIdSet(params TValue[] values)
        {
            TValue value;
            for (int i = values.Length - 1; i >= 0; --i)
            {
                if ((value = values[i]) != null)
                {
                    _values[value.Id.Value] = value;
                    _count++;
                }
            }
        }

        [Impl(256)] public bool ContainsKey(int id) => _values[id] != null;
        [Impl(256)] public bool ContainsKey(Id<TId> id) => _values[id.Value] != null;
        [Impl(256)] public bool Contains(TValue value) => _values[value.Id.Value] != null;

        [Impl(256)] public bool TryGet(int index, out TValue value) => (value = _values[index]) != null;
        [Impl(256)] public bool TryGet(Id<TId> id, out TValue value) => (value = _values[id.Value]) != null;

        public IEnumerator<TValue> GetEnumerator() => _count == 0 ? EmptyEnumerator<TValue>.Instance : new SetEnumerator<TValue>(_values, IdType<TId>.Count, _version);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
