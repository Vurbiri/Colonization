using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class ReadOnlyArray<TValue> : IReadOnlyList<TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] 
        private TValue[] _values;
        private int _count;

        public TValue this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] 
            get => _values[index];
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyArray(TValue[] values)
        {
            _values = values;
            _count = values.Length;
        }

        private ReadOnlyArray() { }

        [MethodImpl(256)] public TValue Rand() => _values[UnityEngine.Random.Range(0, _count)];

        [MethodImpl(256)] public TValue Prev(int index) => _values[(index == 0 ? _count : index) - 1];
        [MethodImpl(256)] public TValue Next(int index) => _values[(index + 1) % _values.Length];

        [MethodImpl(256)] public int LeftIndex(int index) => (index == 0 ? _count : index) - 1;
        [MethodImpl(256)] public int RightIndex(int index) => (index + 1) % _count;

        public IEnumerator<TValue> GetEnumerator() => new ArrayEnumerator<TValue>(_values, _count);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<TValue>(_values, _count);

        public void OnAfterDeserialize() => _count = _values.Length;
        public void OnBeforeSerialize() { }
    }
}
