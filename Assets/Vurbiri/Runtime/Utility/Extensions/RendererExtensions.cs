using UnityEngine;

namespace Vurbiri
{
    public static class RendererExtensions
    {
        public static void SetSharedMaterial(this Renderer self, Material material, int id = 0)
        {
            if (id == 0)
            {
                self.sharedMaterial = material;
            }
            else
            {
                Material[] materials = self.sharedMaterials;
                materials[id] = material;
                self.sharedMaterials = materials;
            }
        }
    }
}
