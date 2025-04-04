//Assets\Vurbiri\Runtime\Utility\Extensions\ExtensionsTransform.cs
using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsTransform
    {
        public static void SetLocalPositionAndRotation(this Transform self, Transform other) 
                                                                                       => self.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
        public static void SetPositionAndRotation(this Transform self, Transform other) => self.SetPositionAndRotation(other.position, other.rotation);
    }
}
