using UnityEngine;

public class CityGraphicSides : ACityGraphic
{
    [SerializeField] protected EnumHashSet<LinkType, CityGraphicSide> _graphicSides;
    [Space]
    [SerializeField] protected RendererSetupGroup[] _renderersSetupGroups;

    protected Players _players;

    public override void Initialize()
    {
        _players = Players.Instance;

        foreach (var side in _graphicSides)
            side.Initialize();
    }

    public override void Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
    {
        Initialize();

        Material material = _players[owner].Material;
        foreach (var group in _renderersSetupGroups)
            group.SetMaterial(material);

        LinkType type; 
        foreach (var link in links)
        {
            type = link.Type; owner = link.Owner;
            AddLink(type);
            if (owner != PlayerType.None)
                RoadBuilt(type, owner);
        }
    }

    public override void AddLink(LinkType type) => _graphicSides[type].AddLink();

    public override void RoadBuilt(LinkType type, PlayerType owner) => _graphicSides[type].SetMaterial(_players[owner].Material);

}
