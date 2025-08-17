using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class ReadOnlyArray<TValue> : IReadOnlyList<TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private TValue[] _values;
        
        private int _count;

        public TValue this[int index]
        {
            [Impl(256)] get => _values[index];
        }

        public int Count
        {
            [Impl(256)] get => _count;
        }

        public ReadOnlyArray(TValue[] values)
        {
            _values = values;
            _count = values.Length;
        }
        private ReadOnlyArray() { }

        [Impl(256)] public TValue Rand() => _values[UnityEngine.Random.Range(0, _count)];

        [Impl(256)] public TValue Prev(int index) => _values[LeftIndex(index)];
        [Impl(256)] public TValue Next(int index) => _values[RightIndex(index)];

        [Impl(256)] public int LeftIndex(int index) => (index == 0 ? _count : index) - 1;
        [Impl(256)] public int RightIndex(int index) => (index + 1) % _count;

        [Impl(256)] public IEnumerator<TValue> GetEnumerator() => new ArrayEnumerator<TValue>(_values, _count);
        [Impl(256)] IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<TValue>(_values, _count);

        [Impl(256)] public static implicit operator ReadOnlyArray<TValue>(TValue[] values) => new(values);

        public void OnAfterDeserialize() => _count = _values != null ? _values.Length : -1;
        public void OnBeforeSerialize() 
        {
#if UNITY_EDITOR
            OnAfterDeserialize();
#endif
        }

#if UNITY_EDITOR
        public void SetValue_EditorOnly(int index, TValue value) => _values[index] = value;
#endif
    }
}
