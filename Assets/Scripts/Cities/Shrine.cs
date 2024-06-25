using System.Collections.Generic;

public class Shrine : ACity
{
    public override CityType Type => CityType.Shrine;
    public override bool IsUpgrade => false;

    public override bool Upgrade(PlayerType owner, IEnumerable<LinkType> linkTypes, out ACity city)
    {
        city = this;
        return false;
    }
}
