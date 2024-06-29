using UnityEngine;

public class ShrineGraphic : CityGraphic
{
    public override void Upgrade(EnumHashSet<LinkType, CrossroadLink> links)
    {
        Initialize();

        Material material = _players.Current.Material;

        foreach (var link in links)
            AddLink(link.Type);

        foreach (var side in _graphicSides) 
            side.SetMaterial(material);
    }
}
