using UnityEngine;

public class CityGraphic : MonoBehaviour
{
    [SerializeField] protected EnumHashSet<LinkType, CityGraphicSide> _graphicSides;
    [Space]
    [SerializeField] protected RendererSetupGroup[] _renderersSetupGroups;

    protected Players _players;

    public virtual void Initialize()
    {
        _players = Players.Instance;

        foreach (var side in _graphicSides)
            side.Initialize();
    }

    public virtual void Upgrade(EnumHashSet<LinkType, CrossroadLink> links)
    {
        Initialize();

        Material material = _players.Current.Material;
        foreach (var group in _renderersSetupGroups)
            group.SetMaterial(material);

        LinkType type; PlayerType owner;
        foreach (var link in links)
        {
            type = link.Type; owner = link.Owner;
            AddLink(type);
            if (owner != PlayerType.None)
                RoadBuilt(type, owner);
        }
    }

    public virtual void AddLink(LinkType type) => _graphicSides[type].AddLink();

    public virtual void RoadBuilt(LinkType type, PlayerType owner) => _graphicSides[type].RoadBuilt(_players[owner].Material);

}
