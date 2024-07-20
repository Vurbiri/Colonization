using UnityEngine;

public class ShrineGraphic : CityGraphicSides
{
    public override void Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
    {
        Initialize();

        Material material = _players[owner].Material;

        foreach (var link in links)
            AddLink(link.Type);

        foreach (var side in _graphicSides) 
            side.SetMaterial(material);
    }
}
