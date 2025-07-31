using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
	public static class VectorExtensions
	{
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(this Vector3 self, Vector3 other)
        {
            float x = self.x - other.x, y = self.y - other.y, z = self.z - other.z;
            return x * x + y * y + z * z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(this Vector2 self, Vector2 other)
        {
            float x = self.x - other.x, y = self.y - other.y;
            return x * x + y * y;
        }
    }
}
