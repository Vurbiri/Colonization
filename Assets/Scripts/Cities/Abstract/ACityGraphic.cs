using System.Collections.Generic;
using UnityEngine;

public abstract class ACityGraphic : MonoBehaviour
{
    [SerializeField] protected EnumArray<LinkType, ACityGraphicSide> _graphicSide;

    public abstract void Initialize();

    public abstract void AddLink(LinkType type);
    public abstract void RoadBuilt(LinkType type, int countFreeLink);
}
