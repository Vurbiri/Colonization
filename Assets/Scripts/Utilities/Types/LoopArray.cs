using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopArray<T> : IEnumerator<T>, IEnumerable<T>
{
    private readonly T[] _array;
    private T _current;
    private readonly int _count = 0;
    private int _cursor = 0, _countCurrent;

    public int Count => _count;
    public int Cursor { get => _cursor; set => _cursor = value; }

    object IEnumerator.Current => _current;
    public T Current => _current;
    public T Next => _current = _array[_cursor = (_cursor + 1) % _count];
    public T Rand => _current = _array[_cursor = Random.Range(0, _count)];

    public LoopArray(T[] array)
    {
        _count = _countCurrent = array.Length;
        _array = array;
    }

    public void SetCursor(T obj)
    {
        for(int i = 0; i < _count; i++) 
        {
            if (_array[i].Equals(obj))
            {
                _cursor = i;
                return;
            }
        }
    }

    public int SetRandCursor() => _cursor = Random.Range(0, _count);

    public bool MoveNext()
    {
        if (--_countCurrent < 0)
            return false;

        _current = _array[_cursor];
        _cursor = (_cursor + 1) % _count;
        return true;
    }
    public void Reset() => _countCurrent = _count;
    public void Dispose() { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator()
    {
        _countCurrent = _count;
        return this;
    }
}
