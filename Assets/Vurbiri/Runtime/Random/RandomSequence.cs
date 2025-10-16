using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public class RandomSequence : IEnumerator<int>, IEnumerable<int>
    {
        private readonly int[] _ids;
        private readonly int _count;
        private int _cursor = 0, _current;

        public int this[int index] { [Impl(256)] get => _ids[index]; }

        public int Count { [Impl(256)] get => _count; }

        public int Next
        {
            get
            {
                _current = _ids[_cursor++];

                if (_cursor == _count)
                    Shuffle();

                return _current;
            }
        }

        public int Current { [Impl(256)] get => _current; }
        object IEnumerator.Current { [Impl(256)] get => _current; }

        public RandomSequence(int count) : this(0, count) { }
        public RandomSequence(int min, int max)
        {
            _count = max - min;
            _ids = new int[_count];

            _ids[0] = min;
            for (int i = 1, j; i < _count; i++)
            {
                _ids[i] = i + min;

                j = SysRandom.Next(i);
                (_ids[j], _ids[i]) = (_ids[i], _ids[j]);
            }
        }

        public RandomSequence(IReadOnlyList<int> ids)
        {
            _count = ids.Count;
            _ids = new int[_count];

            _ids[0] = ids[0];
            for (int i = 1, j; i < _count; i++)
            {
                _ids[i] = ids[i];

                j = SysRandom.Next(i);
                (_ids[j], _ids[i]) = (_ids[i], _ids[j]);
            }
        }

        public void Shuffle()
        {
            _cursor = 0;
            for (int i = 1, j; i < _count; i++)
            {
                j = Random.Range(0, i);
                (_ids[j], _ids[i]) = (_ids[i], _ids[j]);
            }
        }

        public bool MoveNext()
        {
            if(_cursor < _count)
            {
                _current = _ids[_cursor++];
                return true;
            }
            else
            {
                Shuffle();
                return false;
            }
        }

        [Impl(256)] public void Reset() => Shuffle();

        public void Dispose() { }


        public IEnumerator<int> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
