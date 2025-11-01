using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class IdSet<TId, TValue> : ReadOnlyIdSet<TId, TValue> where TId : IdType<TId> where TValue : class, IValueId<TId>
    {
        public new TValue this[int id]     { [Impl(256)] get => _values[id];       [Impl(256)] set => Replace(value); }
        public new TValue this[Id<TId> id] { [Impl(256)] get => _values[id.Value]; [Impl(256)] set => Replace(value); }

        public IdSet() { }

        [Impl(256), JsonConstructor]
        public IdSet(IReadOnlyList<TValue> list) : base(list) { }
        [Impl(256)]
        public IdSet(params TValue[] values) : base(values) { }

        [Impl(256)] public bool TryAdd(TValue value)
        {
            bool result = _values[value.Id] == null;
            if (result)
            {
                _values[value.Id] = value;
                _count++;
            }
            return result;
        }

        [Impl(256)] public void Add(TValue value)
        {
            if (!TryAdd(value)) 
                Errors.AddItem(value?.ToString());
        }

        [Impl(256)] public void Replace(TValue value)
        {
            if (_values[value.Id] == null)
                _count++;

            _values[value.Id] = value;
        }

        [Impl(256)] public void ReplaceRange(IEnumerable<TValue> collection)
        {
            foreach (TValue value in collection)
                Replace(value);
        }
    }
}
