using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
	public class ArrayEnumerator<T> : IEnumerator<T>
	{
        private readonly T[] _values;
        private readonly int _count;
        private int _cursor = 0;
        private T _current;

        public T Current => _current;

        object IEnumerator.Current => _current;

        public ArrayEnumerator(T[] values)
        {
            _values = values;
            _count = values.Length;
        }

        public bool MoveNext()
        {
            if (_cursor >= _count)
                return false;

            _current = _values[_cursor++];
            return true;
        }

        public void Reset()
        {
            _cursor = 0;
        }

        public void Dispose() { }
    }
}
