using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public static class CONST
    {
        public const int MAX_CIRCLES = 4;
        public const int MAX_HEXAGONS = ((HEX_COUNT_SIDES * MAX_CIRCLES * (MAX_CIRCLES + 1)) >> 1) + 1;

        public const int CHANCE_WATER = 17;

        public const float PI = Mathf.PI;
        public const float TAU = 2f * PI;

        public const float COS_00 = 1f;
        public const float COS_30 = 0.86602540378f;
        public const float COS_60 = 0.5f;
        public const float COS_90 = 0f;

        public const float SIN_00 = COS_90;
        public const float SIN_30 = COS_60;
        public const float SIN_60 = COS_30;
        public const float SIN_90 = COS_00;

        public const int HEX_COUNT_SIDES = 6;
        public const int HEX_COUNT_VERTICES = HEX_COUNT_SIDES;
        public const float HEX_DIAMETER_OUT = 22f;
        public const float HEX_RADIUS_OUT = HEX_DIAMETER_OUT * 0.5f;
        public const float HEX_DIAMETER_IN = HEX_DIAMETER_OUT * COS_30;
        public const float HEX_RADIUS_IN = HEX_DIAMETER_IN * 0.5f;

        public static readonly IReadOnlyList<Vector3> HEX_VERTICES;
        public static readonly IReadOnlyList<Vector3> VERTEX_DIRECTIONS;
        public static readonly IReadOnlyList<Vector3> HEX_SIDES;
        public static readonly IReadOnlyList<Vector3> SIDE_DIRECTIONS;

        public static readonly IReadOnlyList<float> COS_HEX = new float[] { COS_30, COS_30, COS_90, -COS_30, -COS_30, -COS_90 };
        public static readonly IReadOnlyList<float> SIN_HEX = new float[] { -SIN_30, SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90 };
        public static readonly IReadOnlyList<float> COS_HEX_DIRECT = new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60};
        public static readonly IReadOnlyList<float> SIN_HEX_DIRECT = new float[] { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 };

        public static readonly IReadOnlyList<Key> NEAR_HEX = new Key[]{ new(2, 0), new(1, 1), new(-1, 1), new(-2, 0), new(-1, -1), new(1, -1) };
        public static readonly IReadOnlyList<Key> NEAR_HEX_TWO;

        public static readonly IReadOnlyDictionary<Key, Quaternion> ACTOR_ROTATIONS;

        public static readonly IReadOnlyList<Quaternion> LINK_ROTATIONS 
            = new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, -120f, 0f), Quaternion.Euler(0f, 0f, 0f) };

        public const int ID_GATE = 13;
        public static readonly IReadOnlyList<int> NUMBERS = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 };


        static CONST()
        {
            Vector3[] positions = new Vector3[HEX_COUNT_VERTICES];
            Vector3[] directions = new Vector3[HEX_COUNT_VERTICES];
            for (int i = 0; i < HEX_COUNT_VERTICES; i++)
            {
                directions[i] = new Vector3(COS_HEX[i], 0, SIN_HEX[i]);
                positions[i] = HEX_RADIUS_OUT * directions[i];
            }
            VERTEX_DIRECTIONS = directions;
            HEX_VERTICES = positions;

            directions = new Vector3[HEX_COUNT_SIDES];
            positions = new Vector3[HEX_COUNT_SIDES];
            for (int i = 0; i < HEX_COUNT_SIDES; i++)
            {
                directions[i] = new Vector3(COS_HEX_DIRECT[i], 0, SIN_HEX_DIRECT[i]);
                positions[i] = HEX_DIAMETER_IN * directions[i];
            }
            SIDE_DIRECTIONS = directions;
            HEX_SIDES = positions;

            Dictionary<Key, Quaternion> quaternions = new(HEX_COUNT_SIDES);
            float angle = 90f;
            for (int i = 0; i < HEX_COUNT_SIDES; i++)
            {
                quaternions[NEAR_HEX[i]] = Quaternion.Euler(0f, angle, 0f);
                angle -= 60f;
            }
            ACTOR_ROTATIONS = quaternions;

            Key[] nearTwo= new Key[HEX_COUNT_SIDES << 1];
            Key key;
            for (int i = 0, j = 0; i < HEX_COUNT_SIDES; i++, j = i << 1)
            {
                key = NEAR_HEX[i];
                nearTwo[j] = key + key;
                nearTwo[j + 1] = key + NEAR_HEX.Next(i);
            }
            NEAR_HEX_TWO = nearTwo;
        }
    }
}

