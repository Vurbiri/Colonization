using UnityEngine;

public class CityGraphic : MonoBehaviour
{
    [SerializeField] protected EnumHashSet<LinkType, ACityGraphicSide> _graphicSides;
    [Space]
    [SerializeField] protected Renderer[] _renderersSetMaterial;

    private Material _material;

    public virtual void Initialize()
    {
        foreach(var side in _graphicSides)
            side.Initialize();
    }

    public virtual void Upgrade(CityGraphic graphic)
    {
        Initialize();
        SetMaterial(graphic._material);
    }

    public virtual void SetMaterial(Material material)
    {
        _material = material;

        foreach (var renderer in _renderersSetMaterial)
            renderer.sharedMaterial = material;
    }

    public virtual void AddLink(LinkType type) => _graphicSides[type].AddLink();

    public virtual void RoadBuilt(LinkType type) => _graphicSides[type].RoadBuilt();
}
