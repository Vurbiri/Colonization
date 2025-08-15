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
    }
}
