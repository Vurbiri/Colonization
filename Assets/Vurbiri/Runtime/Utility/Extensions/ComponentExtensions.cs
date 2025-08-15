using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class ComponentExtensions
	{

        [Impl(256)] public static bool TryGetComponentInChildren<T>(this Component self, out T component, bool includeInactive = true)
        {
            component = self.GetComponentInChildren<T>(includeInactive);
            return component != null;
        }

        [Impl(256)] public static bool TryGetComponentInParent<T>(this Component self, out T component, bool includeInactive = true)
        {
            component = self.GetComponentInParent<T>(includeInactive);
            return component != null;
        }

        [Impl(256)] public static bool TryGetComponentInFirstParent<T>(this Component self, out T component)
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
