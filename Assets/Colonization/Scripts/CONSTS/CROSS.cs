using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
	public class CROSS
	{
        public const int MAX_COUNT = 3;

        public static readonly Key RightDown = new( 2, -1); // Right + Down
        public static readonly Key RightUp   = new( 2,  1); // Right + Up
        public static readonly Key Up        = new( 0,  2); // Up + Up
        public static readonly Key LeftUp    = new(-2,  1); // Left  + Up
        public static readonly Key LeftDown  = new(-2, -1); // Left  + Down
        public static readonly Key Down      = new( 0, -2); // Down  + Down

        public static readonly ReadOnlyArray<Key> NEAR = new(new Key[] { RightDown, RightUp, Up, LeftUp, LeftDown, Down });

        public static readonly ReadOnlyArray<Quaternion> LINK_ROTATIONS
            = new(new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, -120f, 0f), Quaternion.Euler(0f,   0f, 0f) });
        public static readonly ReadOnlyArray<Quaternion> LINK_MIRROR
            = new(new Quaternion[] { Quaternion.Euler(0f, 300f, 0f), Quaternion.Euler(0f,   60f, 0f), Quaternion.Euler(0f, 180f, 0f) });
    }
}
