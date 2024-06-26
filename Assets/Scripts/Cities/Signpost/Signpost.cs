using System.Collections.Generic;
using UnityEngine;

public class Signpost : ACity
{
    [Space]
    [SerializeField] private CitiesScriptable _prefabs;

    public override CityType Type => CityType.Signpost;
    public override PlayerType Owner => PlayerType.None;

    public override bool Setup()
    {
        if (!base.Setup())
            return false;

        _prefabNextUpgrade = _isGate ? _prefabs[CityType.Shrine] : _prefabs[CityType.Camp];
        _graphic.gameObject.SetActive(true);
        return true;
    }

    public override bool Upgrade(PlayerType owner, IEnumerable<CrossroadLink> links, out ACity city)
    {
        _owner = owner;
        return base.Upgrade(owner, links, out city);
    }

    public override void Show(bool isShow) => _graphic.gameObject.SetActive(isShow);
}
