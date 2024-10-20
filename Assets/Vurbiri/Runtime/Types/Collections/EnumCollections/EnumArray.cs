using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [Serializable, JsonArray]
    public class EnumArray<TType, TValue> : IEnumerable<TValue>
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif 
         where TType : Enum
    {
        [SerializeField] protected TValue[] _values;
        protected int _count;

        public int Count => _count;

        public virtual TValue this[TType type] { get => _values[type.ToInt()]; set => _values[type.ToInt()] = value; }
        public virtual TValue this[int index] { get => _values[index]; set => _values[index] = value; }

        public IReadOnlyList<TValue> Values => _values;

        public EnumArray()
        {
            _count = Enum<TType>.Count;
            _values = new TValue[_count];
        }

        public EnumArray(TValue defaultValue)
        {
            _count = Enum<TType>.Count;
            _values = new TValue[_count];

            for (int i = 0; i < _count; i++)
                _values[i] = defaultValue;
        }

        public EnumArray(IReadOnlyList<TValue> collection)
        {
            _count = Enum<TType>.Count;
            _values = new TValue[_count];

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

        public static implicit operator EnumArray<TType, TValue>(TValue[] value) => new(value);

        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public virtual void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _count = Enum<TType>.Count;
            if (_values.Length != _count)
                Array.Resize(ref _values, _count);
        }
        public void OnAfterDeserialize() { }
#endif
        #endregion
    }
}
