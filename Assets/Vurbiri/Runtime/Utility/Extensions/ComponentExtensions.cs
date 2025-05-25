using UnityEngine;

namespace Vurbiri
{
    public static class ComponentExtensions
	{

        public static bool TryGetComponentInChildren<T>(this Component self, out T component, bool includeInactive = true)
        {
            component = self.GetComponentInChildren<T>(includeInactive);
            return component != null;
        }

        public static bool TryGetComponentInParent<T>(this Component self, out T component, bool includeInactive = true)
        {
            component = self.GetComponentInParent<T>(includeInactive);
            return component != null;
        }

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
