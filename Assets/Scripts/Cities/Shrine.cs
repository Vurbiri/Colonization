using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : ACity
{
    public override CityType Type => CityType.Shrine;

    public override void RoadBuilt(LinkType type, int countFreeLink)
    {

    }
}
