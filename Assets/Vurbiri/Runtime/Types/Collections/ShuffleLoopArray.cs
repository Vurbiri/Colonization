//Assets\Vurbiri\Runtime\Types\Collections\ShuffleLoopArray.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Collections
{
    public class ShuffleLoopArray<T>
    {
        protected readonly T[] _array;
        protected readonly int _count;
        protected T _current;
        protected int _cursor = 0;

        public T Next
        {
            get
            {
                _current = _array[_cursor];

                if (++_cursor == _count)
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

        protected ShuffleLoopArray(int count)
        {
            _count = count;
            _array = new T[_count];
        }

        public ShuffleLoopArray(IReadOnlyList<T> array)
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
}
