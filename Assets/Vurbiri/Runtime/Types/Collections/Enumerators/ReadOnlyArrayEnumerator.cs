using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
	public struct ReadOnlyArrayEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _values;
        private readonly int _count;
        private int _cursor;
        private T _current;

        public readonly T Current { [Impl(256)] get => _current; }
        readonly object IEnumerator.Current { [Impl(256)] get => _current; }

        public ReadOnlyArrayEnumerator(T[] values, int count)
        {
            _values = values;
            _count = count;
            _cursor = -1;
            _current = default;
        }

        public bool MoveNext()
        {
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
