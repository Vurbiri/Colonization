using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class IdArray<TId, TValue> : ReadOnlyIdArray<TId, TValue> where TId : IdType<TId>
    {
        public TValue[] Values { [Impl(256)] get => _values; }

        public new TValue this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; [Impl(256)] set => _values[id.Value] = value; }
        public new TValue this[int index]  { [Impl(256)] get => _values[index];    [Impl(256)] set => _values[index] = value; }

        public IdArray() { }
        public IdArray(TValue defaultValue) : base(defaultValue) { }
        public IdArray(Func<TValue> factory) : base(factory) { }
        [JsonConstructor]
        public IdArray(IReadOnlyList<TValue> list) : base(list) { }

        public static implicit operator IdArray<TId, TValue>(TValue[] value) => new(value);
        public static implicit operator IdArray<TId, TValue>(List<TValue> value) => new(value);
    }
}
