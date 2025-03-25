//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\IdHashSet.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public partial class IdSet<TId, TValue> : IReadOnlyList<TValue> where TId : IdType<TId> where TValue : class, IValueId<TId>
    {
        [SerializeField] private TValue[] _values;
        [SerializeField] private int _count;
        private readonly int _capacity;

        public int Filling => _count;
        public int Count => _capacity;

        public TValue this[int id] { get => _values[id]; set => Replace(value); }
        public TValue this[Id<TId> id] { get => _values[id.Value]; set => Replace(value); }

        public IdSet()
        {
            _capacity = IdType<TId>.Count;
            _count = 0;

            _values = new TValue[_capacity];
        }

        public IdSet(IEnumerable<TValue> collection) : this()
        {
            foreach (TValue value in collection)
                Add(value);
        }

        public bool ContainsKey(int id) => _values[id] != null;
        public bool ContainsKey(Id<TId> id) => _values[id.Value] != null;
        public bool Contains(TValue value) => _values[value.Id.Value] != null;

        public bool TryAdd(TValue value)
        {
            int index = value.Id.Value;

            if (_values[index] != null)
                return false;

            _values[index] = value;
            _count++;
            return true;
        }

        public void Add(TValue value)
        {
            if (TryAdd(value)) 
                return;

            Errors.AddItem(value.ToString());
        }

        public void Replace(TValue value)
        {
            int index = value.Id.Value;

            if (_values[index] == null)
                _count++;

            _values[index] = value;
        }

        public void ReplaceRange(IEnumerable<TValue> collection)
        {
            foreach (TValue value in collection)
                Replace(value);
        }

        public TValue Next(int index)
        {
            TValue value;
            int start = index;
            do
            {
                index = ++index % _capacity;
                value = _values[index];
                if (value != null)
                    return value;
            }
            while (index != start);

            return value;
        }

        public bool TryGetValue(int index, out TValue value)
        {
            foreach (TValue v in this)
            {
                if (index-- == 0)
                {
                    value = v;
                    return v != null;
                }
            }
            value = null;
            return false;

        }
        public bool TryGetValue(Id<TId> id, out TValue value) => TryGetValue(id.Value, out value);

        public List<TValue> GetRange(int start, int end)
        {
            List<TValue> values = new(end - start + 1);
            TValue value;

            for (int i = start; i <= end; i++)
            {
                value = _values[i];
                if (value != null)
                    values.Add(value);
            }

            return values;
        }

        public IEnumerator<TValue> GetEnumerator() => new IdSetEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new IdSetEnumerator(this);

        #region Nested classes: IdSetEnumerator
        //***********************************
        public class IdSetEnumerator : IEnumerator<TValue>
        {
            private readonly TValue[] _values;
            private readonly int _capacity;
            private int _cursor = -1;
            private TValue _current;

            public TValue Current => _current;
            object IEnumerator.Current => _current;

            public IdSetEnumerator(IdSet<TId, TValue> parent)
            {
                _values = parent._values;
                _capacity = parent._capacity;
            }

            public bool MoveNext()
            {
                if (++_cursor >= _capacity)
                    return false;

                _current = _values[_cursor];

                if (_current == null)
                    return MoveNext();

                return true;
            }

            public void Reset() => _cursor = -1;

            public void Dispose() { }
        }
        #endregion
    }
}
