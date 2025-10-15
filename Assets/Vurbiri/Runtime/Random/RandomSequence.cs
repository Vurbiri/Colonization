using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public class RandomSequence
    {
        private readonly int[] _ids;
        protected readonly int _count;
        protected int _cursor = 0;

        public int this[int index] { [Impl(256)] get => _ids[index]; }

        public int Count { [Impl(256)] get => _count; }

        public int Next
        {
            get
            {
                int current = _ids[_cursor++];

                if (_cursor == _count)
                    Shuffle();

                return current;
            }
        }

        public RandomSequence(int count)
        {
            _count = count;
            _ids = new int[count];

            for (int i = 1, j; i < count; i++)
            {
                _ids[i] = i;

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
    }
}
