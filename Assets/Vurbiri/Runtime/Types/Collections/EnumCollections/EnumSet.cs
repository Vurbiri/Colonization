//Assets\Vurbiri\Runtime\Types\Collections\EnumCollections\EnumSet.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public partial class EnumSet<TType, TValue> : IReadOnlyList<TValue> where TType : Enum where TValue : class, IValueTypeEnum<TType>
    {
        [SerializeField] private TValue[] _values;
        [SerializeField] private int _count;

        public int Filling => _count;
        public int Count => _capacity;

        public IEnumerable<TType> Types => _typesEnumerable;
        public TValue this[int index] { get => _values[index]; set => Replace(value); }
        public TValue this[TType type] { get => _values[type.ToInt()]; set => Replace(value); }

        private readonly int _capacity;
        private readonly EnumHashSetKeysEnumerable _typesEnumerable;

        public EnumSet()
        {
            _capacity = _count = 0;
            foreach (TType item in Enum<TType>.Values)
            {
                if (item.ToInt() >= 0)
                    _capacity++;
            }

            _values = new TValue[_capacity];
            _typesEnumerable = new(this);
        }

        public EnumSet(IEnumerable<TValue> collection) : this()
        {
            foreach (TValue value in collection)
                Add(value);
        }

        public bool ContainsKey(TType type) => _values[type.ToInt()] != null;
        public bool Contains(TValue value) => _values[value.Type.ToInt()] != null;

        public void Add(TValue value)
        {
            if (TryAdd(value)) return;

            Errors.AddItem(value.ToString());
        }

        public bool TryAdd(TValue value)
        {
            int index = value.Type.ToInt();

            if (_values[index] != null)
                return false;

            _values[index] = value;
            _count++;
            return true;
        }

        public void Replace(TValue value)
        {
            int index = value.Type.ToInt();

            if (_values[index] == null)
                _count++;

            _values[index] = value;
        }

        public bool Remove(TType type)
        {
            int index = type.ToInt();

            if (_values[index] == null)
                return false;

            _values[index] = null;
            _count--;
            return true;
        }

        public TValue First()
        {
            TValue value = null;

            for (int i = 0; i < _capacity; i++)
            {
                value = _values[i];
                if (value != null)
                    return value;
            }
            return value;
        }

        public TValue Next(TType type)
        {
            TValue value;
            int index = type.ToInt(), start = index;
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

        public TValue GetValue(int index)
        {
            foreach (TValue value in this)
                if (index-- == 0)
                    return value;
            
            Errors.IndexOutOfRange(index);
            return null;
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

        public bool TryGetValue(TType type, out TValue value) => TryGetValue(index: type.ToInt(), out value);

        public List<TValue> GetRange(TType typeStart, TType typeEnd)
        {
            int start = typeStart.ToInt(), end = typeEnd.ToInt();
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

        public IEnumerator<TValue> GetEnumerator() => new EnumHashSetValueEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        #region Nested classes: EnumHashSetValueEnumerator, EnumHashSetKeysEnumerable, EnumHashSetKeysEnumerator, AEnumHashSetEnumerator
        //***********************************
        public class EnumHashSetValueEnumerator : AEnumHashSetEnumerator, IEnumerator<TValue>
        {
            public TValue Current => _currentValue;
            object IEnumerator.Current => _currentValue;

            public EnumHashSetValueEnumerator(EnumSet<TType, TValue> parent) : base(parent) { }
        }
        //***********************************
        public class EnumHashSetKeysEnumerable : IEnumerable<TType>
        {
            private readonly EnumSet<TType, TValue> _parent;

            public EnumHashSetKeysEnumerable(EnumSet<TType, TValue> parent) => _parent = parent;

            public IEnumerator<TType> GetEnumerator() => new EnumHashSetKeysEnumerator(_parent);
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        //***********************************
        public class EnumHashSetKeysEnumerator : AEnumHashSetEnumerator, IEnumerator<TType>
        {
            public TType Current => _currentType;
            object IEnumerator.Current => _currentType;

            public EnumHashSetKeysEnumerator(EnumSet<TType, TValue> parent) : base(parent) { }
        }
        //***********************************
        public abstract class AEnumHashSetEnumerator
        {
            private readonly TValue[] _values;
            private readonly int _capacity;
            private int _cursor = -1;
            protected TType _currentType;
            protected TValue _currentValue;

            public AEnumHashSetEnumerator(EnumSet<TType, TValue> parent)
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

                _currentType = _currentValue.Type;

                return true;
            }

            public void Reset() => _cursor = -1;

            public void Dispose() { }
        }
        #endregion
    }
}

