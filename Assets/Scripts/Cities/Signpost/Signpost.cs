using UnityEngine;

public class Signpost : City
{
    [Space]
    [SerializeField] private CitiesScriptable _prefabs;

    public override bool Setup()
    {
        if (!base.Setup())
            return false;

        _owner = PlayerType.None;

        if (_isGate)
            _prefabNextUpgrade = _prefabs[CityType.Shrine];
        else if (_waterCount > 0)
            _prefabNextUpgrade = _prefabs[CityType.Berth];
        else
            _prefabNextUpgrade = _prefabs[CityType.Camp];

        _graphic.gameObject.SetActive(true);
        return true;
    }

    public override bool Build(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links, out City city)
    {
        _owner = owner;
        if(Upgrade(links, out city))
            return true;

        _owner = PlayerType.None;
        return false;
    }

    public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
}
