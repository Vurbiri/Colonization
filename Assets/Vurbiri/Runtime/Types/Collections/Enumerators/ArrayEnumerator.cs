using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
	public class ArrayEnumerator<T> : IEnumerator<T>
	{
        private readonly T[] _values;
        private readonly Version.Current _version;
        private readonly int _count;
        private int _cursor = -1;
        private T _current;

        public T Current => _current;

        object IEnumerator.Current => _current;

        public ArrayEnumerator(T[] values, int count, Version.Current version)
        {
            _values = values;
            _count = count;
            _version = version;
        }
        public ArrayEnumerator(T[] values, int count) : this(values, count, Version.Static) { }

        public bool MoveNext()
        {
            _version.Validate();

            if (++_cursor < _count)
            {
                _current = _values[_cursor];
                return true;
            }

            _current = default;
            return false;
        }

        public void Reset()
        {
            _cursor = -1;
            _current = default;
        }

        public void Dispose() { }
    }
}
