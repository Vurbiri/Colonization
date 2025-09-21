using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public class RandomSequence
    {
        private readonly int[] _ids;
        protected readonly int _count;
        protected int _cursor = 0;

        public int this[int index] => _ids[index];

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

                j = SysRandom.Range(0, i);
                (_ids[j], _ids[i]) = (_ids[i], _ids[j]);
            }
        }

        public RandomSequence(int[] ids)
        {
            _count = ids.Length;
            _ids = new int[_count];

            _ids[0] = ids[0];
            for (int i = 1, j; i < _count; i++)
            {
                _ids[i] = ids[i];

                j = SysRandom.Range(0, i);
                (_ids[j], _ids[i]) = (_ids[i], _ids[j]);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
