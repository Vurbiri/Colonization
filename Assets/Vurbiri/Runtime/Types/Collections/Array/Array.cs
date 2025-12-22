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
            [Impl(256)] set
            {
                _values[index] = value;
                _version.Next();
            }
        }

        [Impl(256)] public Array(int count) : base(count) { }
        [Impl(256)] public Array(params TValue[] values) : base(values) { }
        [Impl(256), JsonConstructor] public Array(ICollection<TValue> collection) : base(collection) { }

        public void Resize(int newSize)
        {
            if (_count != newSize)
            {
                Array.Resize(ref _values, newSize);
                _count = newSize;
                _version.Next();
            }
        }

        [Impl(256)] public void Import(TValue[] values)
        {
            _values = values;
            _count = values.Length;
            _version.Next();
        }

        [Impl(256)] public static implicit operator Array<TValue>(TValue[] values) => new(values);
    }
}
