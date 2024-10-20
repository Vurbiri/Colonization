using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [Serializable]
    public class EnumHashSet<TType, TValue> :
#if UNITY_EDITOR
        ISerializationCallbackReceiver,
#endif
        IEnumerable<TValue> where TType : Enum where TValue : class, IValueTypeEnum<TType>
    {
        [SerializeField] private TValue[] _values;
        [SerializeField] private int _count;

        public int Count => _count;
        public int Capacity => _capacity;

        public IEnumerable<TType> Types => _typesEnumerable;
        public TValue this[TType type] { get => _values[type.ToInt(_offset)]; set => Replace(value); }

        private readonly int _offset, _capacity;
        private readonly EnumHashSetKeysEnumerable _typesEnumerable;

        public EnumHashSet()
        {
            int min = Int32.MaxValue, max = Int32.MinValue, key;
            foreach (TType item in Enum<TType>.Values)
            {
                key = item.ToInt();
                if (key < 0) continue;

                min = key < min ? key : min;
                max = key > max ? key : max;
            }

            _offset = -min;
            _capacity = max - min + 1;
            _count = 0;

            _values = new TValue[_capacity];
            _typesEnumerable = new(this);
        }

        public EnumHashSet(IEnumerable<TValue> collection) : this()
        {
            foreach (TValue value in collection)
                Add(value);
        }

        public bool ContainsKey(TType type) => _values[type.ToInt(_offset)] != null;
        public bool Contains(TValue value) => _values[value.Type.ToInt(_offset)] != null;

        public void Add(TValue value)
        {
            if (TryAdd(value)) return;

            throw new Exception($"������ ���� {value.Type} ��� ��� ��������.");
        }

        public bool TryAdd(TValue value)
        {
            int index = value.Type.ToInt(_offset);

            if (_values[index] != null)
                return false;

            _values[index] = value;
            _count++;
            return true;
        }

        public void Replace(TValue value)
        {
            int index = value.Type.ToInt(_offset);

            if (_values[index] == null)
                _count++;

            _values[index] = value;
        }

        public bool Remove(TType type)
        {
            int index = type.ToInt(_offset);

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

        public int FirstEmptyIndex()
        {
            for (int i = 0; i < _capacity; i++)
                if (_values[i] == null)
                    return i;
            return -1;
        }

        public TValue Next(TType type)
        {
            TValue value;
            int index = type.ToInt(_offset), start = index;
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

            throw new IndexOutOfRangeException();
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

            throw new IndexOutOfRangeException();
        }

        public bool TryGetValue(TType type, out TValue value) => TryGetValue(index: type.ToInt(), out value);

        public List<TValue> GetRange(TType typeStart, TType typeEnd)
        {
            int start = typeStart.ToInt(_offset), end = typeEnd.ToInt(_offset);
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

                    if (value.Type.Equals(_values[j].Type))
                        _values[j] = null;
                }

                index = value.Type.ToInt(_offset);
                if (index == i)
                {
                    _count++;
                    continue;
                }

                (_values[i], _values[index]) = (_values[index], _values[i]);
                i--;
            }
        }

        public void OnAfterDeserialize() { }
#endif
        #endregion


        public IEnumerator<TValue> GetEnumerator() => new EnumHashSetValueEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        #region Nested classes: EnumHashSetValueEnumerator, EnumHashSetKeysEnumerable, EnumHashSetKeysEnumerator, AEnumHashSetEnumerator
        //***********************************
        public class EnumHashSetValueEnumerator : AEnumHashSetEnumerator, IEnumerator<TValue>
        {
            public TValue Current => _currentValue;
            object IEnumerator.Current => _currentValue;

            public EnumHashSetValueEnumerator(EnumHashSet<TType, TValue> parent) : base(parent) { }
        }
        //***********************************
        public class EnumHashSetKeysEnumerable : IEnumerable<TType>
        {
            private readonly EnumHashSet<TType, TValue> _parent;

            public EnumHashSetKeysEnumerable(EnumHashSet<TType, TValue> parent) => _parent = parent;

            public IEnumerator<TType> GetEnumerator() => new EnumHashSetKeysEnumerator(_parent);
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        //***********************************
        public class EnumHashSetKeysEnumerator : AEnumHashSetEnumerator, IEnumerator<TType>
        {
            public TType Current => _currentType;
            object IEnumerator.Current => _currentType;

            public EnumHashSetKeysEnumerator(EnumHashSet<TType, TValue> parent) : base(parent) { }
        }
        //***********************************
        public abstract class AEnumHashSetEnumerator
        {
            private readonly TValue[] _values;
            private readonly int _capacity;
            private int _cursor = -1;
            protected TType _currentType;
            protected TValue _currentValue;

            public AEnumHashSetEnumerator(EnumHashSet<TType, TValue> parent)
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

