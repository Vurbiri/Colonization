using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
	public struct ArrayEnumerator<T> : IEnumerator<T>
	{
        private readonly T[] _values;
        private readonly Version.Current _version;
        private readonly int _count;
        private int _cursor;
        private T _current;

        public readonly T Current { [Impl(256)] get => _current; }
        readonly object IEnumerator.Current { [Impl(256)] get => _current; }

        public ArrayEnumerator(T[] values, int count, Version.Current version)
        {
            _values = values;
            _version = version;
            _count = count;
            _cursor = -1;
            _current = default;
        }

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

        [Impl(256)]
        public void Reset()
        {
            _cursor = -1;
            _current = default;
        }

        [Impl(256)]
        public readonly void Dispose() { }
    }
}
