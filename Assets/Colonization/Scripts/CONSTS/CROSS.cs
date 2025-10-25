using System;
using UnityEngine;
using Vurbiri.Collections;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization
{
	public static class CROSS
	{
        public const int MAX = HEX.VERTICES * MAX_CIRCLES * MAX_CIRCLES;

        public static readonly Key RightDown = new( 1, -1); // Right + Down
        public static readonly Key RightUp   = new( 1,  1); // Right + Up
        public static readonly Key Up        = new( 0,  2); // Up    + Up
        public static readonly Key LeftUp    = new(-1,  1); // Left  + Up
        public static readonly Key LeftDown  = new(-1, -1); // Left  + Down
        public static readonly Key Down      = new( 0, -2); // Down  + Down

        public static readonly ReadOnlyArray<Vector3> DIRECTIONS;

        public static readonly ReadOnlyArray<Key> NEAR = new(new Key[] { RightDown, RightUp, Up, LeftUp, LeftDown, Down });

        public static readonly ReadOnlyArray<Quaternion> LINK_ROTATIONS;
        public static readonly ReadOnlyArray<Quaternion> LINK_MIRROR;

        public static int Distance(Key a, Key b)
        {
            int x = Math.Abs(a.x - b.x), y = Math.Abs(a.y - b.y);
            return (x < y) ? (x + y) >> 1 : x;
        }
        public static Key ToHex(Key c, int type)
        {
            int offset;
            if (c.y > 0)
                offset = (c.y & 1) - ((1 - type) << 1) & MathI.NotEqual(c.y, 2);
            else
                offset = (type << 1) - (c.y & 1) & MathI.NotEqual(c.y, -2);

            return new(c.x, (c.y / 4 << 1) + offset);
        }

        static CROSS()
        {
            DIRECTIONS = new( new Vector3[] 
            { 
                new( COS_30, 0f, -SIN_30),
                new( COS_30, 0f,  SIN_30),
                new( COS_90, 0f,  SIN_90),
                new(-COS_30, 0f,  SIN_30),
                new(-COS_30, 0f, -SIN_30),
                new(-COS_90, 0f, -SIN_90),
            });

            LINK_ROTATIONS = new(new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, -120f, 0f), Quaternion.Euler(0f,   0f, 0f) });
            LINK_MIRROR    = new(new Quaternion[] { Quaternion.Euler(0f, 300f, 0f), Quaternion.Euler(0f,   60f, 0f), Quaternion.Euler(0f, 180f, 0f) });
        }
    }
}
