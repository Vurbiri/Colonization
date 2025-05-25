using UnityEngine;

namespace Vurbiri
{
    public static class TransformExtensions
    {
        public static void SetLocalPositionAndRotation(this Transform self, Transform other)
        {
            self.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
        }

        public static void SetPositionAndRotation(this Transform self, Transform other)
        {
            self.SetPositionAndRotation(other.position, other.rotation);
        }
    }
}
