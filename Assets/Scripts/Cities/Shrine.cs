

public class Shrine : ACity
{
    public override CityType Type => CityType.Shrine;
    public override PlayerType Owner => _owner;

    protected PlayerType _owner;

    public override void RoadBuilt(LinkType type, int countFreeLink)
    {

    }
}
