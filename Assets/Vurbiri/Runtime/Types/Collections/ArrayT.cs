using Newtonsoft.Json;
using System;
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

        [Impl(256)] public Array(TValue[] values)
        {
            _values = values;
            _count = values.Length;
        }

        [Impl(256)] public Array(int count)
        {
            _values = new TValue[count];
            _count = count;
        }
    }
}
