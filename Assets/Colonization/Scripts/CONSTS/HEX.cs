using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Vurbiri.Collections;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
    public static class HEX
    {
        public const int SIDES = 6;
        public const int VERTICES = 6;

        public const int MAX = ((SIDES * MAX_CIRCLES * (MAX_CIRCLES + 1)) >> 1) + 1;

        public const int GATE = 7;
        public static readonly ReadOnlyArray<int> IDS = new(new int[]{ 2, 3, 4, 5, 6, 8, 9, 10, 11, 12 });

        public const float DIAMETER_OUT = 22f;
        public const float RADIUS_OUT = DIAMETER_OUT * 0.5f;
        public const float DIAMETER_IN = DIAMETER_OUT * COS_30;
        public const float RADIUS_IN = DIAMETER_IN * 0.5f;

        public static readonly Key LeftUp    = new(-1,  1); // Left  + Up
        public static readonly Key Left      = new(-2,  0); // Left  + Left
        public static readonly Key LeftDown  = new(-1, -1); // Left  + Down
        public static readonly Key RightDown = new( 1, -1); // Right + Down
        public static readonly Key Right     = new( 2,  0); // Right + Right
        public static readonly Key RightUp   = new( 1,  1); // Right + Up

        public static readonly ReadOnlyArray<Vector3> DIRECTIONS;
        public static readonly ReadOnlyDictionary<Key, Quaternion> ROTATIONS;

        public static readonly ReadOnlyArray<Key> NEAR = new(new Key[] { Right, RightUp, LeftUp, Left, LeftDown, RightDown });
        public static readonly ReadOnlyArray<Key> NEAR_TWO;
        public static readonly ReadOnlyArray<Key> NEAR_THREE;

        public static readonly HearHexagons NEARS;

        public static readonly ReadOnlyArray<float> COS = new(new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60 });
        public static readonly ReadOnlyArray<float> SIN = new(new float[] { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 });

        static HEX()
        {
            var directions = new Vector3[SIDES];
            for (int i = 0; i < SIDES; i++)
                directions[i] = new Vector3(COS[i], 0, SIN[i]);
            DIRECTIONS = new(directions);

            Dictionary<Key, Quaternion> quaternions = new(SIDES);
            float angle = 90f;
            for (int i = 0; i < SIDES; i++)
            {
                quaternions[NEAR[i]] = Quaternion.Euler(0f, angle, 0f);
                angle -= 60f;
            }
            ROTATIONS = new(quaternions);

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

        public class HearHexagons : IReadOnlyList<ReadOnlyArray<Key>>
        {
            private readonly ReadOnlyArray<Key>[] _values;

            public Key Random => _values.Rand().Rand();

            public int Count => 3;

            public ReadOnlyArray<Key> this[int index] => _values[index];

            public HearHexagons(ReadOnlyArray<Key> a, ReadOnlyArray<Key> b, ReadOnlyArray<Key> c)
            {
                _values = new ReadOnlyArray<Key>[] { a, b, c };
            }

            public IEnumerator<ReadOnlyArray<Key>> GetEnumerator() => new ArrayEnumerator<ReadOnlyArray<Key>>(_values, 3);
            IEnumerator IEnumerable.GetEnumerator() => new ArrayEnumerator<ReadOnlyArray<Key>>(_values, 3);
        }
    }
}
