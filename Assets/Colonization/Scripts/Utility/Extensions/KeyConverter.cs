//Assets\Colonization\Scripts\Utility\Extensions\KeyConverter.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public static class KeyConverter
    {
        private static readonly Vector2 s_offsetHex = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);
        private static readonly Vector2 s_offsetCross = new(HEX_RADIUS_OUT * COS_30, HEX_RADIUS_OUT * SIN_30);

        public static Key HexPositionToKey(this Vector3 position) => new(2f * position.x / s_offsetHex.x, position.z / s_offsetHex.y);
        public static Vector3 HexKeyToPosition(this Key key) => new(0.5f * s_offsetHex.x * key.X, 0f, s_offsetHex.y * key.Y);

        public static Key CrossPositionToKey(this Vector3 position) => new(2f * position.x / s_offsetCross.x, position.z / s_offsetCross.y);
        public static Vector3 CrossKeyToPosition(this Key key) => new(0.5f * s_offsetCross.x * key.X, 0f, s_offsetCross.y * key.Y);
    }
}
