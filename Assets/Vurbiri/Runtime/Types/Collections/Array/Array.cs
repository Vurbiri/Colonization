using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class Array<TValue> : ReadOnlyArray<TValue>
    {
        public new TValue this[int index]
        {
            [Impl(256)] get => _values[index];
            [Impl(256)] set => _values[index] = value;
        }

        [Impl(256)] public Array(int count) : base(count) { }
        [Impl(256)] public Array(params TValue[] values) : base(values) { }
        [Impl(256), JsonConstructor] public Array(IReadOnlyList<TValue> list) : base(list) { }

        public void Resize(int newSize)
        {
            if (_count != newSize)
            {
                TValue[] newArr = new TValue[newSize];
                int count = Math.Min(newSize, _count);
                for (int i = 0; i < count; i++)
                    newArr[i] = _values[i];

                _values = newArr;
                _count = newSize;
            }
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < _count; i++)
                array[i] = _values[i];
        }

        [Impl(256)] public static implicit operator Array<TValue>(TValue[] values) => new(values);
      
    }
}
