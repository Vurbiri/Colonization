using UnityEngine;

public class LoopArray<T>
{
    private readonly T[] _array;
    private readonly int _default = 0, _count = 0;
    private int _cursor = 0;

    public int Count => _count;
    public int Cursor { get => _cursor; set => _cursor = value; }

    public T Default => _array[_cursor = _default];
    public T Forward
    {
        get
        {
           if(++_cursor >= _count) _cursor = 0;

            return _array[_cursor];
        }
    }
    public T Back
    {
        get
        {
            if (--_cursor < 0) _cursor = _count - 1;

            return _array[_cursor];
        }
    }

    public T Rand => _array[_cursor = Random.Range(0, _count)];

    public LoopArray(T[] array)
    {
        _count = array.Length;
        _array = new T[_count];

        _array = array;
        //for(int i = 0; i < _count; i++)
        //    _array[i] = array[i];
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
}
