using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class ReadOnlyIdArray<TId, TValue> : IReadOnlyList<TValue> where TId : IdType<TId>
    {
        [SerializeField] protected TValue[] _values = new TValue[IdType<TId>.Count];

        public int Count { [Impl(256)]  get => IdType<TId>.Count; }

        public TValue this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; }
        public TValue this[int index]  { [Impl(256)]  get => _values[index]; }

        #region Constructors
        protected ReadOnlyIdArray() { }
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
        public ReadOnlyIdArray(params TValue[] values)
        {
            int count = Mathf.Min(IdType<TId>.Count, values.Length);
            for (int i = 0; i < count; i++)
                _values[i] = values[i];
        }
        [JsonConstructor]
        public ReadOnlyIdArray(IReadOnlyList<TValue> list)
        {
            int count = Mathf.Min(IdType<TId>.Count, list.Count);
            for (int i = 0; i < count; i++)
                _values[i] = list[i];
        }
        #endregion

        public IEnumerator<TValue> GetEnumerator() => new ArrayEnumerator<TValue>(_values, IdType<TId>.Count);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<TValue>(_values, IdType<TId>.Count);

        public static implicit operator ReadOnlyIdArray<TId, TValue>(TValue[] value) => new(value);
        public static implicit operator ReadOnlyIdArray<TId, TValue>(List<TValue> value) => new(value);

        public static implicit operator ReadOnlyArray<TValue>(ReadOnlyIdArray<TId, TValue> self) => new(self._values);
    }
}
