//Assets\Vurbiri\Runtime\Utility\Extensions\ExtensionsRenderer.cs
using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsRenderer
    {
        public static void SetSharedMaterial(this Renderer self, Material material, int id = 0)
        {
            if (id == 0)
            {
                self.sharedMaterial = material;
                return;
            }

            Material[] materials = self.sharedMaterials;
            materials[id] = material;
            self.sharedMaterials = materials;
        }
    }
}
