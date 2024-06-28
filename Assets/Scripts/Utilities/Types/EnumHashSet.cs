using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnumHashSet<TType, TValue> : ISerializationCallbackReceiver, IReadOnlyCollection<TValue>
       where TType : Enum
       where TValue : class, IValueTypeEnum<TType>
{
    [SerializeField] private TValue[] _values;
    [SerializeField] private int _count;
    [SerializeField] private int _countMax;

    public int Count => _count;
    public int CountMax => _countMax;
    public int Capacity => _capacity;

    public IEnumerable<TType> Types => _typesEnumerable;
    public TValue this[TType type] => _values[type.ToInt(_offset)];

    private readonly int _offset, _capacity;
    private readonly EnumHashSetKeysEnumerable _typesEnumerable;

    public EnumHashSet()
    {
        int min = Int32.MaxValue, max = Int32.MinValue, key;
        _countMax = 0;
        foreach (TType item in Enum<TType>.GetValues())
        {
            key = item.ToInt();
            if (key < 0) continue;

            min = key < min ? key : min;
            max = key > max ? key : max;
            _countMax++;
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

    public void Add(TValue value)
    {
        if (TryAdd(value))
            return;

        Debug.LogError($"Объект типа {value.Type} уже был добавлен!");
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

    public void Remove(TType type) => _values[type.ToInt(_offset)] = null;

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

    public TValue GetValue(int index)
    {
        foreach (TValue value in this)
            if (index-- == 0)
                return value;

        throw new IndexOutOfRangeException();
    }

    public void OnBeforeSerialize() 
    {
        if(_values.Length != _capacity)
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

        List<TType> types = new(Enum<TType>.GetValues());
        types.RemoveAll((t) => t.ToInt() < 0);

        _countMax = types.Count;
    }

    public void OnAfterDeserialize() { }

    public IEnumerator<TValue> GetEnumerator() => new EnumHashSetEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Nested classes: EnumHashSetEnumerator, EnumHashSetKeysEnumerable, EnumHashSetKeysEnumerator
    //***********************************
    public class EnumHashSetEnumerator : IEnumerator<TValue>
    {
        private readonly TValue[] _values;
        private int _capacity, _cursor = -1;
        private TValue _current;

        public TValue Current => _current;
        object IEnumerator.Current => _current;

        public EnumHashSetEnumerator(EnumHashSet<TType, TValue> parent)
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

        public void Reset()
        {
            _cursor = -1;
        }

        public void Dispose() { }
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
    public class EnumHashSetKeysEnumerator : IEnumerator<TType>
    {
        private readonly TValue[] _values;
        private int _capacity, _cursor = -1;
        private TType _current;

        public TType Current => _current;
        object IEnumerator.Current => _current;

        public EnumHashSetKeysEnumerator(EnumHashSet<TType, TValue> parent)
        {
            _values = parent._values;
            _capacity = parent._capacity;
        }

        public bool MoveNext()
        {
            if (++_cursor >= _capacity)
                return false;

            _current = _values[_cursor].Type;

            if (_current == null)
                return MoveNext();

            return true;
        }

        public void Reset()
        {
            _cursor = -1;
        }

        public void Dispose() { }
    }
    #endregion
}

