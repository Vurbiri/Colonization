using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Collections
{
	internal struct SetEnumerator<T> : IEnumerator<T> where T : class
	{
        private readonly T[] _values;
        private readonly Version.Current _version;
        private readonly int _count;
        private int _cursor;
        private T _current;

        public readonly T Current => _current;

        readonly object IEnumerator.Current => _current;

        public SetEnumerator(T[] values, int count, Version.Current version)
        {
            _values = values;
            _count = count;
            _version = version;
            _cursor = 0;
            _current = null;
        }
        public SetEnumerator(T[] values, int count) : this(values, count, Version.Static) { }

        public bool MoveNext()
        {
            _version.Validate();

            bool canMoveNext; T current = null;
            while ((canMoveNext = _cursor < _count) && (current = _values[_cursor++]) == null);

            _current = current;
            return canMoveNext;
        }

        public void Reset()
        {
            _cursor = 0;
            _current = null;
        }

        public readonly void Dispose() { }
    }
}
