using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Road
    {
		private class Links : IReadOnlyList<Key>
		{
            private const int BASE_CAPACITY = 7;

            private Key[] _values = new Key[BASE_CAPACITY];
            private int _capacity = BASE_CAPACITY;
            private int _count;
            private int _start, _end;

            public Links(Crossroad start, Crossroad end)
            {
                _end = _capacity >> 1;
                _start = _end - 1;

                _values[_start] = start.Key;
                _values[_end++] = end.Key;
                _count = 2;
            }

            public Key this[int index] => _values[_start + index];

            public int Count { [Impl(256)] get => _count; }
            public Key Start { [Impl(256)] get => _values[_start]; }
            public Key End { [Impl(256)] get => _values[_end - 1]; }

            public void Add(Crossroad crossroad)
            {
                if (_end == _capacity)
                    GrowArray(_capacity);

                _values[_end++] = crossroad.Key;
                _count++;
            }
            public void Insert(Crossroad crossroad)
            {
                if (_start == 0)
                    GrowArray(_capacity);

                _values[--_start] = crossroad.Key;
                _count++;
            }

            public void AddRange(Links other)
            {
                int addCount = other._count - 1;
                if (_end + addCount > _capacity)
                    GrowArray(_capacity + addCount, -(addCount >> 1));

                for (int i = _end, j = other._start + 1; j < other._end; i++, j++)
                    _values[i] = other._values[j];
                
                _count += addCount;
                _end = _start + _count;
            }
            public void AddReverseRange(Links other)
            {
                int addCount = other._count - 1;
                if (_end + addCount > _capacity)
                    GrowArray(_capacity + addCount, -(addCount >> 1));

                for (int i = _end, j = other._end - 2; j >= other._start; i++, j--)
                    _values[i] = other._values[j];

                _count += addCount;
                _end = _start + _count;
            }

            public void InsertRange(Links other)
            {
                int addCount = other._count - 1;
                if (_start - addCount < 0)
                    GrowArray(_capacity + addCount, addCount >> 1);

                for (int i = _start - 1, j = other._end - 2; j >= other._start; i--, j--)
                    _values[i] = other._values[j];

                _count += addCount;
                _start = _end - _count;
            }
            public void InsertReverseRange(Links other)
            {
                int addCount = other._count - 1;
                if (_start - addCount < 0)
                    GrowArray(_capacity + addCount, addCount >> 1);

                for (int i = _start - 1, j = other._start + 1; j < other._end; i--, j++)
                    _values[i] = other._values[j];

                _count += addCount;
                _start = _end - _count;
            }

            public void Remove()
            {
                _count--; _end--;
            }
            public void Extract()
            {
                _count--; _start++;
            }

            private void GrowArray(int count, int startOffset = 0)
            {
                while (_capacity <= count)
                    _capacity = _capacity << 1 | BASE_CAPACITY;

                int start = ((_capacity - _count) >> 1) + startOffset;
                var array = new Key[_capacity];

                for (int i = _start, j = start; i < _end; i++, j++)
                    array[j] = _values[i];

                _values = array;
                _start = start; 
                _end = _start + _count;
            }

            public IEnumerator<Key> GetEnumerator()
            {
                for (int i = _start; i < _end; i++)
                    yield return _values[i];
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                for (int i = _start; i < _end; i++)
                    yield return _values[i];
            }

        }
    }
}
