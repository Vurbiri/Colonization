using System.Collections.Generic;
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
        _prefabNextUpgrade = _isGate ? _prefabs[CityType.Shrine] : _prefabs[CityType.Camp];
        _graphic.gameObject.SetActive(true);
        return true;
    }

    public override bool Build(PlayerType owner, Material material, IEnumerable<CrossroadLink> links, out City city)
    {
        _owner = owner;
        _graphic.SetMaterial(material);
        if(Upgrade(links, out city))
            return true;

        _owner = PlayerType.None;
        return false;
    }

    public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
}
