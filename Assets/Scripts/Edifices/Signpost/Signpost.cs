using UnityEngine;

public class Signpost : AEdifice
{
    public override bool Build(AEdifice prefab, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out AEdifice city)
    {
        _prefabUpgrade = prefab;
        return Upgrade(owner, links, out city);
    }

    public override bool Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out AEdifice city)
    {
        _owner = owner;
        if (base.Upgrade(owner, links, out city))
            return true;

        _owner = PlayerType.None;
        return false;
    }

    public override void Setup(AEdifice edifice)
    {
        _prefabUpgrade = edifice;
        _typeNext = edifice.Type;
        _isUpgrade = true;

        //Debug.Log(edifice.Type);
    }

    public override bool CanBuyUpgrade(PlayerType owner, Currencies cash) => base.CanBuyUpgrade(PlayerType.None, cash);

    public override void AddLink(LinkType type) => _graphic.AddLink(type);

    public override void AddRoad(LinkType type, PlayerType owner) => _graphic.RoadBuilt(type, owner);

    public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
}
