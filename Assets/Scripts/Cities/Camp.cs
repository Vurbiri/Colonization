using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camp : ACity
{
    public override CityType Type => CityType.Camp;
    public override PlayerType Owner => _owner;

    protected PlayerType _owner;

    public override void RoadBuilt(LinkType type, int countFreeLink)
    {

    }
}
