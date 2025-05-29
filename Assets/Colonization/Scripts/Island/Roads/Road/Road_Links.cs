using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    public partial class Road
    {
		private class Links : IReadOnlyList<Crossroad>
		{
            private const int BASE_CAPACITY = 7;

            private Crossroad[] _values = new Crossroad[BASE_CAPACITY];
            private int _capacity = BASE_CAPACITY;
            private int _count;
            private int _start, _end;

            public Links(Crossroad start, Crossroad end)
            {
                _end = _capacity >> 1;
                _start = _end - 1;

                _values[_start] = start;
                _values[_end++] = end;
                _count = 2;
            }

            public Crossroad this[int index] => _values[index + _start];

            public int Count => _count;
            public Crossroad Start => _values[_start];
            public Crossroad End => _values[_end - 1];

            
            public void Add(Crossroad crossroad)
            {
                if (_end == _capacity)
                    GrowArray(_capacity);

                _values[_end++] = crossroad;
                _count++;
            }
            public void Insert(Crossroad crossroad)
            {
                if (_start == 0)
                    GrowArray(_capacity);

                _values[--_start] = crossroad;
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
                _values[--_end] = null;
                _count--;
            }
            public void Extract()
            {
                _values[_start++] = null;
                _count--;
            }

            private void GrowArray(int count, int startOffset = 0)
            {
                while (_capacity <= count)
                    _capacity = _capacity << 1 | BASE_CAPACITY;

                int start = ((_capacity - _count) >> 1) + startOffset;
                Crossroad[] array = new Crossroad[_capacity];

                for (int i = _start, j = start; i < _end; i++, j++)
                    array[j] = _values[i];

                _values = array;
                _start = start; 
                _end = _start + _count;
            }

            public IEnumerator<Crossroad> GetEnumerator()
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
