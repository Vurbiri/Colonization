using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnumArray<TType, TValue> : ISerializationCallbackReceiver, IReadOnlyList<TValue>
    where TType : Enum
    where TValue : class, IValueTypeEnum<TType>
{
    [SerializeField] private TValue[] _values;

    public int Count => _count;
    public int Capacity => _capacity;

    public IEnumerable<TType> Types => _typesEnumerable;

    public TValue this[TType type] => _values[type.ToInt(_offset)];
    public TValue this[int index]
    {
        get 
        {
            foreach(TValue value in this)
                if(index-- == 0)
                    return value;

            throw new IndexOutOfRangeException();
        }
    }

    private readonly int _offset, _capacity;
    private int _count;
    private readonly EnumArrayKeysEnumerable _typesEnumerable;

    public EnumArray()
    {
        int min = Int32.MaxValue, max = Int32.MinValue, key;

        foreach (TType item in Enum<TType>.GetValues())
        {
            key = item.ToInt();
            min = key < min ? key : min;
            max = key > max ? key : max;
        }

        _offset = -min;
        _capacity = max - min + 1;
        _count = 0;

        _values = new TValue[_capacity];
        _typesEnumerable = new(this);
    }

    public EnumArray(IEnumerable<TValue> collection) : this()
    {
        foreach (TValue value in collection)
            TryAdd(value);
    }

    public void Add(TValue value)
    {
        int index = value.Type.ToInt(_offset);

        if (_values[index] != null)
        {
            Debug.LogError($"Объект типа {value.Type} уже был добавлен!");
            return;
        }

        _values[value.Type.ToInt(_offset)] = value;
        _count++;
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

    public TValue First()
    {
        for(int i = 0; i < _capacity; i++)
            if (_values[i] != null)
                return _values[i];
        
        return null;
    }

    public void OnBeforeSerialize() 
    {
        if(_values.Length != _capacity)
            Array.Resize(ref _values, _capacity);

        TValue value;
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
                continue;

            (_values[i], _values[index]) = (_values[index], _values[i]);
            i--;
        }
    }

    public void OnAfterDeserialize() 
    {
        _count = 0;
        for (int i = 0; i < _values.Length; i++)
            if (_values[i] != null)
                _count++;
    }

    public IEnumerator<TValue> GetEnumerator() => new EnumArrayEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region Nested classes: EnumArrayEnumerator, EnumArrayKeysEnumerator
    //***********************************
    public class EnumArrayEnumerator : IEnumerator<TValue>
    {
        private readonly TValue[] _values;
        private int _capacity, _cursor = -1;
        private TValue _current;

        public TValue Current => _current;
        object IEnumerator.Current => _current;

        public EnumArrayEnumerator(EnumArray<TType, TValue> parent)
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
    public class EnumArrayKeysEnumerable : IEnumerable<TType>
    {
        private readonly EnumArray<TType, TValue> _parent;

        public EnumArrayKeysEnumerable(EnumArray<TType, TValue> parent) => _parent = parent;

        public IEnumerator<TType> GetEnumerator() => new EnumArrayKeysEnumerator(_parent);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    //***********************************
    public class EnumArrayKeysEnumerator : IEnumerator<TType>
    {
        private readonly TValue[] _values;
        private int _capacity, _cursor = -1;
        private TType _current;

        public TType Current => _current;
        object IEnumerator.Current => _current;

        public EnumArrayKeysEnumerator(EnumArray<TType, TValue> parent)
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

