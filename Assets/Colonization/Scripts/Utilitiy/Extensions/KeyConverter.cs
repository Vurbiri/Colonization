using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public static class KeyConverter
    {
        private static readonly Vector2 _offsetHex = new(HEX_DIAMETER_IN, HEX_DIAMETER_IN * SIN_60);
        private static readonly Vector2 _offsetCross = new(HEX_RADIUS_OUT * COS_30, HEX_RADIUS_OUT * SIN_30);

        public static Key HexPositionToKey(this Vector3 position) => new(2f * position.x / _offsetHex.x, position.z / _offsetHex.y);
        public static Vector3 HexKeyToPosition(this Key key) => new(0.5f * _offsetHex.x * key.X, 0f, _offsetHex.y * key.Y);

        public static Key CrossPositionToKey(this Vector3 position) => new(2f * position.x / _offsetCross.x, position.z / _offsetCross.y);
        public static Vector3 CrossKeyToPosition(this Key key) => new(0.5f * _offsetCross.x * key.X, 0f, _offsetCross.y * key.Y);
    }
}
