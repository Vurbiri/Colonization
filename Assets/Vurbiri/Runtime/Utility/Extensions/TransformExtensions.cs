using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class TransformExtensions
    {
        [Impl(256)] public static void SetLocalPositionAndRotation(this Transform self, Transform other)
        {
            self.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
        }
        
        [Impl(256)] public static void SetPositionAndRotation(this Transform self, Transform other)
        {
            self.SetPositionAndRotation(other.position, other.rotation);
        }
    }
}
