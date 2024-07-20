using UnityEngine;

public abstract class ACityGraphic : MonoBehaviour
{

    public abstract void Initialize();

    public abstract void Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links);

    public abstract void AddLink(LinkType type);

    public abstract void RoadBuilt(LinkType type, PlayerType owner);
}
