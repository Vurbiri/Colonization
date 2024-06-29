using System.Collections.Generic;
using UnityEngine;

public class ShuffleLoopArray<T>
{ 
    private readonly IList<T> _array;
    private T _current;
    private readonly int _count;
    private int _cursor = 0;

    public T Value
    {
        get
        {
            _current = _array[_cursor];

            if(++_cursor == _count)
            {
                _cursor = 0;

                for (int i = 1, j; i < _count; i++)
                {
                    j = Random.Range(0, i);
                    (_array[j], _array[i]) = (_array[i], _array[j]);
                }
            }

            return _current;
        }
    } 

    public ShuffleLoopArray(IList<T> array)
    {
        _count = array.Count;
        _array = new T[_count];

        _array[0] = array[0];
        for (int i = 1, j; i < _count; i++)
        {
            _array[i] = array[i];

            j = Random.Range(0, i);
            (_array[j], _array[i]) = (_array[i], _array[j]);
        }
    }
}
