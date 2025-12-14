using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
	public struct ReadOnlySetEnumerator<T> : IEnumerator<T> where T : class
    {
        private readonly T[] _values;
        private readonly int _count;
        private int _cursor;
        private T _current;

        public readonly T Current { [Impl(256)] get => _current; }
        readonly object IEnumerator.Current { [Impl(256)] get => _current; }

        public ReadOnlySetEnumerator(T[] values, int count)
        {
            _values = values;
            _count = count;
            _cursor = -1;
            _current = null;
        }

        public bool MoveNext()
        {
            bool canMoveNext; T current = null;
            while ((canMoveNext = ++_cursor < _count) && (current = _values[_cursor]) == null) ;

            _current = current;
            return canMoveNext;
        }

        [Impl(256)]
        public void Reset()
        {
            _cursor = -1;
            _current = null;
        }

        [Impl(256)]
        public readonly void Dispose() { }
    }
}
