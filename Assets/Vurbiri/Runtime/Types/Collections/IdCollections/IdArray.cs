//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\IdArray.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class IdArray<TId, TValue> :
#if UNITY_EDITOR
        ISerializationCallbackReceiver,
#endif
        IReadOnlyList<TValue> where TId : AIdType<TId>
    {
        [SerializeField] protected TValue[] _values;
        protected int _count;

        public int Count => _count;

        public virtual TValue this[Id<TId> id] { get => _values[id.Value]; set => _values[id.Value] = value; }
        public virtual TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        public IdArray()
        {
            _count = AIdType<TId>.Count;
            _values = new TValue[_count];
        }

        public IdArray(TValue defaultValue) : this()
        {
            for (int i = 0; i < _count; i++)
                _values[i] = defaultValue;
        }

        public IdArray(IReadOnlyList<TValue> collection) : this() 
        {
            int count = _count <= collection.Count ? _count : collection.Count;
            for (int i = 0; i < count; i++)
                _values[i] = collection[i];
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator IdArray<TId, TValue>(TValue[] value) => new(value);

        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public virtual void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _count = AIdType<TId>.Count;
            if (_values.Length != _count)
                Array.Resize(ref _values, _count);
        }
        public void OnAfterDeserialize() { }
#endif
        #endregion
    }
}
