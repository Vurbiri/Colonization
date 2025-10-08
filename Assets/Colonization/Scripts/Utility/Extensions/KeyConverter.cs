using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public static class KeyConverter
    {
        private static readonly Vector2 s_offsetHex = new(HEX.DIAMETER_IN, HEX.DIAMETER_IN * SIN_60);
        private static readonly Vector2 s_offsetCross = new(HEX.RADIUS_OUT * COS_30, HEX.RADIUS_OUT * SIN_30);

        public static Key HexPositionToKey(this Vector3 position) => new(2f * position.x / s_offsetHex.x, position.z / s_offsetHex.y);
        public static Vector3 HexKeyToPosition(this Key key) => new(0.5f * s_offsetHex.x * key.x, 0f, s_offsetHex.y * key.y);

        public static Key CrossPositionToKey(this Vector3 position) => new(2f * position.x / s_offsetCross.x, position.z / s_offsetCross.y);
        public static Vector3 CrossKeyToPosition(this Key key) => new(0.5f * s_offsetCross.x * key.x, 0f, s_offsetCross.y * key.y);
    }
}
