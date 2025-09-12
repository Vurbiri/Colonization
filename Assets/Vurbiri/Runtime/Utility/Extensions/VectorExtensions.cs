using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class VectorExtensions
	{
        [Impl(256)] public static float SqrDistance(this Vector3 self, Vector3 other)
        {
            float x = self.x - other.x, y = self.y - other.y, z = self.z - other.z;
            return x * x + y * y + z * z;
        }

        [Impl(256)] public static float SqrDistance(this Vector2 self, Vector2 other)
        {
            float x = self.x - other.x, y = self.y - other.y;
            return x * x + y * y;
        }

        #region Set Axis
        [Impl(256)] public static Vector3 SetX(this Vector3 self, float x)
        {
            self.x = x;
            return self;
        }
        [Impl(256)] public static Vector3 SetY(this Vector3 self, float y)
        {
            self.y = y;
            return self;
        }
        [Impl(256)] public static Vector3 SetZ(this Vector3 self, float z)
        {
            self.z = z;
            return self;
        }

        [Impl(256)] public static Vector2 SetX(this Vector2 self, float x)
        {
            self.x = x;
            return self;
        }
        [Impl(256)] public static Vector2 SetY(this Vector2 self, float y)
        {
            self.y = y;
            return self;
        }
        #endregion

        #region Offset Axis
        [Impl(256)] public static Vector3 OffsetX(this Vector3 self, float x)
        {
            self.x += x;
            return self;
        }
        [Impl(256)] public static Vector3 OffsetY(this Vector3 self, float y)
        {
            self.y += y;
            return self;
        }
        [Impl(256)] public static Vector3 OffsetZ(this Vector3 self, float z)
        {
            self.z += z;
            return self;
        }

        [Impl(256)] public static Vector2 OffsetX(this Vector2 self, float x)
        {
            self.x += x;
            return self;
        }
        [Impl(256)] public static Vector2 OffsetY(this Vector2 self, float y)
        {
            self.y += y;
            return self;
        }
        #endregion
    }
}
