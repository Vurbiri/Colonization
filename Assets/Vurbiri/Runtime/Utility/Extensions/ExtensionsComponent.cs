//Assets\Vurbiri\Runtime\Utility\Extensions\ExtensionsComponent.cs
using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsComponent
	{
		public static bool TryGetComponentInFirstParent<T>(this Component self, out T component)
		{
            Transform parent = self.transform.parent;

            if (parent == null)
            {
                component = default;
                return false;
            }

			return parent.TryGetComponent<T>(out component);
        }

    }
}
