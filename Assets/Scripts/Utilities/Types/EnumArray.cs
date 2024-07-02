using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnumArray<TType, TValue> : ISerializationCallbackReceiver, IEnumerable<TValue> 
                                        where TType : Enum where TValue : struct
{
    [SerializeField] protected TValue[] _values;

    public TValue this[TType type] { get => _values[type.ToInt()]; set => _values[type.ToInt()] = value; }
    public TValue this[int index] { get => _values[index]; set => _values[index] = value; }

    protected readonly int _count;

    public int Count => _count;

    public EnumArray()
    {
        _count = Enum<TType>.Count;
        _values = new TValue[_count];
    }

    public EnumArray(EnumArray<TType, TValue> other) : this() => CopyFrom(other);

    public void CopyFrom(EnumArray<TType, TValue> other)
    {
        for (int i = 0; i < _count; i++)
            _values[i] = other._values[i];
    }

    public virtual void OnBeforeSerialize()
    {
        if (_values.Length != _count)
            Array.Resize(ref _values, _count);
    }
    public void OnAfterDeserialize() { }

    public IEnumerator<TValue> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
            yield return _values[i];
    }
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}
