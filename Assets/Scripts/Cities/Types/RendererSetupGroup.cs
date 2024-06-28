using UnityEngine;

[System.Serializable]
public class RendererSetupGroup
{
    [SerializeField] private int _idMaterial;
    [SerializeField] private MeshRenderer[] _renderers;
    
    public void SetMaterial(Material material)
    {
        if(_idMaterial == 0)
        {
            foreach (var renderer in _renderers)
                renderer.sharedMaterial = material;

            return;
        }
        
        Material[] materials;
        foreach (var renderer in _renderers)
        {
            materials = renderer.sharedMaterials;
            materials[_idMaterial] = material;
            renderer.sharedMaterials = materials;
        }
    }
}
