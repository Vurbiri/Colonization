using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public static class TransformExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPositionAndRotation(this Transform self, Transform other)
        {
            self.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPositionAndRotation(this Transform self, Transform other)
        {
            self.SetPositionAndRotation(other.position, other.rotation);
        }
    }
}
