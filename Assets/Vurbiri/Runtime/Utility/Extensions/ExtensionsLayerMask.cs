//Assets\Vurbiri\Runtime\Utility\Extensions\ExtensionsLayerMask.cs
using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsLayerMask
    {
        public static int ToInt(this LayerMask self)
        {
            int value = self.value >> 1;
            int intValue = 0;

            while (value != 0)
            {
                intValue++;
                value = value >> 1;
            }

            return intValue;   
        }
    }
}
