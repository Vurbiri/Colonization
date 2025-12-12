using System.Collections;
using System.Collections.Generic;

namespace Vurbiri
{
	internal class SetEnumerator<T> : IEnumerator<T> where T : class
	{
        private readonly T[] _values;
        private readonly Version.Current _version;
        private readonly int _count;
        private int _cursor = 0;
        private T _current;

        public T Current => _current;

        object IEnumerator.Current => _current;

        //public SetEnumerator(T[] values, Version.Current version) : this(values, values.Length) { }
        public SetEnumerator(T[] values, int count, Version.Current version)
        {
            _values = values;
            _count = count;
            _version = version;
        }

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

        public void Dispose() { }
    }
}
