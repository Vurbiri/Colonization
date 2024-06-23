using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnumHashSetValue<TKey> where TKey : Enum
{
    public TKey Type { get; }
}

[Serializable]
public class EnumHashSet<TKey, TValue> : ISerializationCallbackReceiver, IEnumerable<TValue>
    where TKey : Enum
    where TValue : class, IEnumHashSetValue<TKey>
{
    [SerializeField] private TValue[] _values;

    public int Count => _capacity;
    public int Filled => _count;

    public TValue this[TKey key] => _values[key.ToInt(_offset)];

    private readonly int _offset, _capacity;
    private int _count;

    public EnumHashSet()
    {
        TKey[] keys = Enum<TKey>.GetValues();
        int min = Int32.MaxValue;

        foreach (TKey key in keys)
            min = Mathf.Min(min, key.ToInt());

        _offset = -min;
        _capacity = keys.Length;
        _values = new TValue[_capacity];

        _count = 0;
    }

    public void Add(TValue value)
    {
        int index = value.Type.ToInt(_offset);

        if (_values[index] != null)
        {
            Debug.LogError($"Объект типа {value.Type} был уже добавлен!");
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
        for (int i = 0; i < _capacity; i++)
            if (_values[i] != null)
                _count++;
    }

    public IEnumerator<TValue> GetEnumerator() => new EnumHashSetEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    #region Nested class: EnumHashSetEnumerator
    //***********************************
    public class EnumHashSetEnumerator : IEnumerator<TValue>
    {
        private readonly TValue[] _values;
        private int _capacity, _cursor = -1;
        private TValue _current;

        public TValue Current => _current;
        object IEnumerator.Current => _current;

        public EnumHashSetEnumerator(EnumHashSet<TKey, TValue> parent)
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
    #endregion
}
