using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : ACity
{
    public override CityType Type => CityType.Camp;

    public override void RoadBuilt(LinkType type, int countFreeLink)
    {

    }
}
