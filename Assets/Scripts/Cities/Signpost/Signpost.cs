using UnityEngine;

public class Signpost : City
{
    [Space]
    [SerializeField] private CitiesScriptable _prefabs;

    public override bool Build(CityType type, PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out City city)
    {
        _prefabUpgrade = _prefabs[type];
        _owner = owner;
        if(Upgrade(links, out city))
            return true;

        _owner = PlayerType.None;
        return false;
    }

    public override bool CanBuyBuilding(CityType type, Currencies cash) => _isUpgrade && _owner == PlayerType.None && _prefabs[type].CanBuyBuilding(type, cash);

    public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
}
