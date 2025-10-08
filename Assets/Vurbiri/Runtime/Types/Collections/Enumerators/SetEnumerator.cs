using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
	internal class SetEnumerator<T> : IEnumerator<T> where T : class
	{
        private readonly T[] _values;
        private readonly int _count;
        private bool _canMoveNext;
        private int _cursor = 0;
        private T _current;

        public T Current => _current;

        object IEnumerator.Current => _current;

        public SetEnumerator(T[] values) : this(values, values.Length) { }
        public SetEnumerator(T[] values, int count)
        {
            _values = values;
            _count = count;
        }

        public bool MoveNext()
        {
            while (_canMoveNext = _cursor < _count && (_current = _values[_cursor++]) == null);

            return _canMoveNext;
        }

        public void Reset() => _cursor = 0;

        public void Dispose() { }
    }
}
