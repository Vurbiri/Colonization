using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
	public class ArrayEnumerator<T> : IEnumerator<T>
	{
        private readonly T[] _values;
        private readonly int _count;
        private bool _canMoveNext;
        private int _cursor = 0;
        private T _current;

        public T Current => _current;

        object IEnumerator.Current => _current;

        public ArrayEnumerator(T[] values) : this(values, values.Length) { }
        public ArrayEnumerator(T[] values, int count)
        {
            _values = values;
            _count = count;
        }

        public bool MoveNext()
        {
            _canMoveNext = _cursor < _count;
            if (_canMoveNext)
                _current = _values[_cursor++];

            return _canMoveNext;
        }

        public void Reset() => _cursor = 0;

        public void Dispose() { }
    }
}
