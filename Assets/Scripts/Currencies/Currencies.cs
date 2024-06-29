using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Currencies : ISerializationCallbackReceiver
{
    [SerializeField] private int[] _values;

    public int this[Resource type] { get => _values[(int)type]; set => _values[(int)type] = value; }
    public int this[int index] { get => _values[index]; set => _values[index] = value; }

    private readonly int _count;

    public Currencies()
    {
        _count = Enum<Resource>.Count;
        _values = new int[_count];
    }


    public void OnBeforeSerialize()
    {
        if (_values.Length != _count)
            Array.Resize(ref _values, _count);
    }
    public void OnAfterDeserialize() { }
}
