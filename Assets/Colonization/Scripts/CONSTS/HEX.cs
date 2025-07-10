using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vurbiri.Colonization
{
    public static class HEX
    {
        public const int SIDES = 6;

        public static readonly Key LeftUp    = new(-1,  1); // Left  + Up
        public static readonly Key Left      = new(-2,  0); // Left  + Left
        public static readonly Key LeftDown  = new(-1, -1); // Left  + Down
        public static readonly Key RightDown = new( 1, -1); // Right + Down
        public static readonly Key Right     = new( 2,  0); // Right + Right
        public static readonly Key RightUp   = new( 1,  1); // Right + Up

        public static readonly ReadOnlyCollection<Key> NEAR = new(new Key[] { Right, RightUp, LeftUp, Left, LeftDown, RightDown });
        public static readonly ReadOnlyCollection<Key> NEAR_TWO;
        public static readonly ReadOnlyCollection<Key> NEAR_THREE;

        public static readonly HearHexagons NEARS;

        static HEX()
        {
            Key[] nearTwo = new Key[SIDES << 1];
            Key key;
            for (int i = 0, j = 0; i < SIDES; i++, j = i << 1)
            {
                key = NEAR[i];
                nearTwo[j] = key + key;
                nearTwo[j + 1] = key + NEAR.Next(i);
            }
            NEAR_TWO = new(nearTwo);

            Key[] nearThree = new Key[SIDES * 3];
            key = NEAR[0];
            nearThree[^1] = key + nearTwo[^1];
            nearThree[0]  = key + nearTwo[0];
            nearThree[1]  = key + nearTwo[1];

            for (int i = 1, j = 2, k = 3; i < SIDES; i++, j = i * 2, k = i * 3)
            {
                key = NEAR[i];
                nearThree[k] = key + nearTwo[j];
                nearThree[k - 1] = key + nearTwo[j - 1];
                nearThree[k + 1] = key + nearTwo[j + 1];
            }
            NEAR_THREE = new(nearThree);

            NEARS = new (NEAR, NEAR_TWO, NEAR_THREE);
        }

        public class HearHexagons : IReadOnlyList<ReadOnlyCollection<Key>>
        {
            private readonly ReadOnlyCollection<Key>[] _values;

            public Key Random => _values.Rand().Rand();

            public int Count => 3;

            public ReadOnlyCollection<Key> this[int index] => _values[index];

            public HearHexagons(ReadOnlyCollection<Key> a, ReadOnlyCollection<Key> b, ReadOnlyCollection<Key> c)
            {
                _values = new ReadOnlyCollection<Key>[] { a, b, c };
            }

            public IEnumerator<ReadOnlyCollection<Key>> GetEnumerator() => new ArrayEnumerator<ReadOnlyCollection<Key>>(_values);
            IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<ReadOnlyCollection<Key>>(_values);
        }
    }
}
