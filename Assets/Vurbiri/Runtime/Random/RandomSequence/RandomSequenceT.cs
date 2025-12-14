using System;
using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public class RandomSequence<T> : IEnumerator<T>, IEnumerable<T>
    {
        protected T[] _values;
        protected int _count, _cursor = -1;
        private T _current;

        public int Count { [Impl(256)] get => _count; }

        public T Current { [Impl(256)] get => _current; }
        object IEnumerator.Current { [Impl(256)] get => _current; }

        public T Next
        {
            [Impl(256)]
            get
            {
                if (++_cursor == _count)
                    Shuffle(0);

                return _current = _values[_cursor];
            }
        }

        #region Constructors
        [Impl(256)] protected RandomSequence(int count)
        {
            Throw.IfLess(count, 1);

            _count = count;
            _values = new T[count];
        }

        public RandomSequence(IReadOnlyList<T> ids) : this(ids.Count)
        {
            _values[0] = ids[0];
            for (int i = 1, j; i < _count; ++i)
            {
                _values[i] = ids[i];

                j = SysRandom.Next(i);
                (_values[j], _values[i]) = (_values[i], _values[j]);
            }
        }
        public RandomSequence(params T[] ids) : this(ids.Length)
        {
            _values[0] = ids[0];
            for (int i = 1, j; i < _count; ++i)
            {
                _values[i] = ids[i];

                j = SysRandom.Next(i);
                (_values[j], _values[i]) = (_values[i], _values[j]);
            }
        }
        #endregion

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

        [Impl(256)] public void Reset()
        {
            if (_cursor != -1)
                Shuffle();
        }

        public void Dispose() 
        {
            for (int i = 0; i < _count; ++i)
                (_values[i] as IDisposable)?.Dispose();
        }

        [Impl(256)] public RandomSequence<T> GetEnumerator()
        {
            Shuffle();
            return this;
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Shuffle(int cursor = -1)
        {
            _cursor = cursor;
            for (int i = 1, j; i < _count; ++i)
            {
                j = UnityEngine.Random.Range(0, i);
                (_values[j], _values[i]) = (_values[i], _values[j]);
            }
        }
    }
}
