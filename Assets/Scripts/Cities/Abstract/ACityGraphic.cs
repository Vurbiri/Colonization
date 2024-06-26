using UnityEngine;

public abstract class ACityGraphic : MonoBehaviour
{
    [SerializeField] protected EnumArray<LinkType, ACityGraphicSide> _graphicSides;

    public virtual void Initialize()
    {
        foreach(var side in _graphicSides)
            side.Initialize();
    }

    public virtual void AddLink(LinkType type) => _graphicSides[type].gameObject.SetActive(true);

    public virtual void RoadBuilt(LinkType type) => _graphicSides[type].RoadBuilt();
}
