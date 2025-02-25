//Assets\Colonization\Scripts\CONSTS\HEX.cs
using System.Collections.Generic;

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

        public static readonly IReadOnlyList<Key> NEAR = new Key[] { Right, RightUp, LeftUp, Left, LeftDown, RightDown };
        public static readonly IReadOnlyList<Key> NEAR_TWO;

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
            NEAR_TWO = nearTwo;
        }
    }
}
