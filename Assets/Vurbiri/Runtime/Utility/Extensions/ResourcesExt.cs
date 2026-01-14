using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public static class ResourcesExt
	{
        [Impl(256)] public static void Unload(this Object asset) => Resources.UnloadAsset(asset);

        [Impl(256)] public static void Unload<T>(ref T asset) where T : UnityEngine.Object
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
                asset = null;
            }
        }
    }
}
