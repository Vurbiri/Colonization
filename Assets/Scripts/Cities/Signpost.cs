using System.Collections.Generic;
using UnityEngine;

public class Signpost : ACity
{
    [Space]
    [SerializeField] private CitiesScriptable _prefabs;

    public override CityType Type => CityType.Signpost;
    public override PlayerType Owner => PlayerType.None;

    public override bool Setup(CityDirection type, ICollection<LinkType> linkTypes)
    {
        if(!base.Setup(type, linkTypes))
            return false;

        _prefabNextUpgrade = _isGate ? _prefabs[CityType.Shrine] : _prefabs[CityType.Camp];
        
        return true;
    }

    public override void RoadBuilt(LinkType type, int countFreeLink)
    {

    }
}
