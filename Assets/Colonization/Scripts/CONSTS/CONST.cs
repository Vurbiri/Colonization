using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public static class CONST
    {
        public const int MAX_CIRCLES = 4;
        public const int MAX_HEXAGONS = ((HEX.SIDES * MAX_CIRCLES * (MAX_CIRCLES + 1)) >> 1) + 1;
        public const int MAX_CROSSROADS = HEX.SIDES * MAX_CIRCLES * MAX_CIRCLES;

        public const int CHANCE_WATER = MAX_CIRCLES * MAX_CIRCLES;

        public const int DEFAULT_MAX_WARRIOR = 5;
        public const int DEFAULT_MAX_DEMONS = DEFAULT_MAX_WARRIOR << 1;
        public const int DEFAULT_MAX_EDIFICES = 6;

        public const int GATE_ID = 13;
        public static readonly int[] HEX_IDS = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 };

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

        public const int HEX_COUNT_VERTICES = HEX.SIDES;

        public const float HEX_DIAMETER_OUT = 22f;
        public const float HEX_RADIUS_OUT = HEX_DIAMETER_OUT * 0.5f;
        public const float HEX_DIAMETER_IN = HEX_DIAMETER_OUT * COS_30;
        public const float HEX_RADIUS_IN = HEX_DIAMETER_IN * 0.5f;

        public static readonly ReadOnlyArray<Vector3> VERTEX_DIRECTIONS;
        public static readonly ReadOnlyArray<Vector3> SIDE_DIRECTIONS;

        public static readonly ReadOnlyArray<float> COS_HEX = new(new float[] { COS_30, COS_30, COS_90, -COS_30, -COS_30, -COS_90 });
        public static readonly ReadOnlyArray<float> SIN_HEX = new(new float[] { -SIN_30, SIN_30, SIN_90, SIN_30, -SIN_30, -SIN_90 });
        public static readonly ReadOnlyArray<float> COS_HEX_DIRECT = new(new float[] { COS_00, COS_60, -COS_60, -COS_00, -COS_60, COS_60});
        public static readonly ReadOnlyArray<float> SIN_HEX_DIRECT = new(new float[] { SIN_00, SIN_60, SIN_60, -SIN_00, -SIN_60, -SIN_60 });
        
        public static readonly ReadOnlyDictionary<Key, Quaternion> ACTOR_ROTATIONS;

        public static readonly ReadOnlyArray<Quaternion> LINK_ROTATIONS 
            = new(new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, -120f, 0f), Quaternion.Euler(0f, 0f, 0f) });

        static CONST()
        {
            Vector3[] directions = new Vector3[HEX_COUNT_VERTICES];
            for (int i = 0; i < HEX_COUNT_VERTICES; i++)
                directions[i] = new Vector3(COS_HEX[i], 0, SIN_HEX[i]);
            VERTEX_DIRECTIONS = new(directions);

            directions = new Vector3[HEX.SIDES];
            for (int i = 0; i < HEX.SIDES; i++)
                directions[i] = new Vector3(COS_HEX_DIRECT[i], 0, SIN_HEX_DIRECT[i]);
            SIDE_DIRECTIONS = new(directions);

            Dictionary<Key, Quaternion> quaternions = new(HEX.SIDES);
            float angle = 90f;
            for (int i = 0; i < HEX.SIDES; i++)
            {
                quaternions[HEX.NEAR[i]] = Quaternion.Euler(0f, angle, 0f);
                angle -= 60f;
            }
            ACTOR_ROTATIONS = new(quaternions);
        }
    }
}

