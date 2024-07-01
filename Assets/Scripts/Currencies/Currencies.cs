using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Currencies : ISerializationCallbackReceiver, IReadOnlyList<int>
{
    [SerializeField] private int[] _values;

    public int this[Resource type] { get => _values[(int)type]; set => _values[(int)type] = value; }
    public int this[int index] { get => _values[index]; set => _values[index] = value; }

    private readonly int _count;

    public int Count => _count;

    public Currencies()
    {
        _count = Enum<Resource>.Count;
        _values = new int[_count];
    }
    public Currencies(Currencies other) : this() => CopyFrom(other);

    public void CopyFrom(Currencies other)
    {
        for (int i = 0; i < _count; i++)
            _values[i] = other._values[i];
    }

    public void Pay(Currencies cost)
    {
        for (int i = 0; i < _count; i++)
            _values[i] -= cost._values[i];
    }

    public static bool operator >(Currencies left, Currencies right) => !(left <= right);
    public static bool operator <(Currencies left, Currencies right) => !(left >= right);
    public static bool operator >=(Currencies left, Currencies right)
    {
        for (int i = 0; i < left._count; i++)
            if (left._values[i] < right._values[i])
                return false;
        return true;
    }
    public static bool operator <=(Currencies left, Currencies right)
    {
        for(int i = 0; i < left._count; i++ )
            if(left._values[i] > right._values[i]) 
                return false;
        return true;
    }
    
    
    public void OnBeforeSerialize()
    {
        if (_values.Length != _count)
            Array.Resize(ref _values, _count);

        for(int i = 0; i < _count; i++)
            if (_values[i] < 0)
                _values[i] = 0;
    }
    public void OnAfterDeserialize() { }

    public IEnumerator<int> GetEnumerator()
    {
        for(int i = 0; i < _count; i++)
            yield return _values[i];
    }
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}
