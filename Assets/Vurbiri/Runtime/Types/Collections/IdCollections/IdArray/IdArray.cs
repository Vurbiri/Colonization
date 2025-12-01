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

        #region Constructors
        public IdArray() { }
        [Impl(256)] public IdArray(TValue defaultValue) : base(defaultValue) { }
        [Impl(256)] public IdArray(Func<TValue> factory) : base(factory) { }
        [Impl(256)] public IdArray(params TValue[] values) : base(values) { }
        [Impl(256), JsonConstructor]
        public IdArray(IReadOnlyList<TValue> list) : base(list) { }
        #endregion

        public void Reset()
        {
            for (int i = 0; i < IdType<TId>.Count; ++i)
                _values[i] = default;
        }

        public static implicit operator IdArray<TId, TValue>(TValue[] value) => new(value);
        public static implicit operator IdArray<TId, TValue>(Roster<TValue> value) => new(value);

        public static implicit operator ReadOnlyArray<TValue>(IdArray<TId, TValue> self) => new(self._values);
        public static implicit operator Array<TValue>(IdArray<TId, TValue> self) => new(self._values);
    }
}
