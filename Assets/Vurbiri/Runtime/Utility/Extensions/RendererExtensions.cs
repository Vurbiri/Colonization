using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class RendererExtensions
    {
        [Impl(256)] public static void SetSharedMaterial(this Renderer self, Material material, int id = 0)
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
