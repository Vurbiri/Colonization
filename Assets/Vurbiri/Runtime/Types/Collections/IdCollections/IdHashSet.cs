//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\IdHashSet.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class IdHashSet<TId, TValue> : IReadOnlyList<TValue>
#if UNITY_EDITOR
        ,ISerializationCallbackReceiver
#endif
         where TId : IdType<TId> where TValue : class, IValueId<TId>
    {
        [SerializeField] private TValue[] _values;
        [SerializeField] private int _count;
        private readonly int _capacity;
        private readonly IdHashSetIdsEnumerable _typesEnumerable;

        public int CountAvailable => _count;
        public int Count => _capacity;

        public IEnumerable<Id<TId>> CurrentIds => _typesEnumerable;
        public TValue this[int id] { get => _values[id]; set => Replace(value); }
        public TValue this[Id<TId> id] { get => _values[id.Value]; set => Replace(value); }

        public IdHashSet()
        {
            _capacity = IdType<TId>.Count;
            _count = 0;

            _values = new TValue[_capacity];
            _typesEnumerable = new(this);
        }

        public IdHashSet(IEnumerable<TValue> collection) : this()
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

            throw new Exception($"Элемент c Id = {value.Id} уже был добавлен.");
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
            if (collection == null)
                throw new ArgumentNullException("IEnumerable<TValue> collection");

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
            throw new IndexOutOfRangeException(GetType().Name, null);
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

        public IEnumerator<TValue> GetEnumerator() => new IdHashSetValueEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region Nested classes: IdHashSetValueEnumerator, IdHashSetIdsEnumerable, IdHashSetIdsEnumerator, AIdHashSetEnumerator
        //***********************************
        public class IdHashSetValueEnumerator : AIdHashSetEnumerator, IEnumerator<TValue>
        {
            public TValue Current => _currentValue;
            object IEnumerator.Current => _currentValue;

            public IdHashSetValueEnumerator(IdHashSet<TId, TValue> parent) : base(parent) { }
        }
        //***********************************
        public class IdHashSetIdsEnumerable : IEnumerable<Id<TId>>
        {
            private readonly IdHashSet<TId, TValue> _parent;

            public IdHashSetIdsEnumerable(IdHashSet<TId, TValue> parent) => _parent = parent;

            public IEnumerator<Id<TId>> GetEnumerator() => new IdHashSetIdsEnumerator(_parent);
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        //***********************************
        public class IdHashSetIdsEnumerator : AIdHashSetEnumerator, IEnumerator<Id<TId>>
        {
            public Id<TId> Current => _currentId;
            object IEnumerator.Current => _currentId;

            public IdHashSetIdsEnumerator(IdHashSet<TId, TValue> parent) : base(parent) { }
        }
        //***********************************
        public abstract class AIdHashSetEnumerator
        {
            private readonly TValue[] _values;
            private readonly int _capacity;
            private int _cursor = -1;
            protected Id<TId> _currentId;
            protected TValue _currentValue;

            public AIdHashSetEnumerator(IdHashSet<TId, TValue> parent)
            {
                _values = parent._values;
                _capacity = parent._capacity;
            }

            public bool MoveNext()
            {
                if (++_cursor >= _capacity)
                    return false;

                _currentValue = _values[_cursor];

                if (_currentValue == null)
                    return MoveNext();

                _currentId = _currentValue.Id;

                return true;
            }

            public void Reset() => _cursor = -1;

            public void Dispose() { }
        }
        #endregion

        #region ISerializationCallbackReceiver
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            if (_values.Length != _capacity)
                Array.Resize(ref _values, _capacity);

            TValue value; _count = 0;
            for (int index, i = 0; i < _capacity; i++)
            {
                value = _values[i];
                if (value == null)
                    continue;

                for (int j = i + 1; j < _capacity; j++)
                {
                    if (_values[j] == null)
                        continue;

                    if (value.Id == _values[j].Id)
                        _values[j] = null;
                }

                index = value.Id.Value;
                if (index == i)
                {
                    _count++;
                    continue;
                }

                (_values[i], _values[index]) = (_values[index], _values[i]);
                i--;
            }
        }

        public void OnAfterDeserialize() 
        {
        }
#endif
        #endregion

    }
}
