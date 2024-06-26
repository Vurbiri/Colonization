using UnityEngine;

public class CrossroadLink : IValueTypeEnum<LinkType>
{
    public KeyDouble Key => _key;
    public LinkType Type => _type;
    public Vector3 Position => _middle;
    public PlayerType Owner {get => _owner; set => _owner = value; }
    public bool IsNotCities => _start.CityType == CityType.Signpost && _end.CityType == CityType.Signpost;

    public Crossroad Start => _start;
    public Crossroad End => _end;
   
    private Crossroad _start, _end;
    private readonly LinkType _type;
    private PlayerType _owner;
    private readonly KeyDouble _key;
    private readonly Vector3 _middle;

    public CrossroadLink(Crossroad crossA, Crossroad crossB, PlayerType owner = PlayerType.None)
    {
        _start = crossA; _end = crossB;
        _type = (LinkType)(_end - _start);

        if (!(_start.AddLink(this) && _end.AddLink(this)))
            return;

        _key = _start & _end;
        _owner = owner;
        _middle = (_start.Position + _end.Position) * 0.5f;
    }

    public void SetStart(Crossroad cross)
    {
        if(_start != cross)
            (_start, _end) = (_end, _start);
    }

    public void RoadBuilt(PlayerType owner)
    {
        _owner = owner;
        _start.RoadBuilt(_type);
        _end.RoadBuilt(_type);
    }

    public Crossroad Other(Crossroad crossroad) => crossroad == _start ? _end : _start;

    public override string ToString() => $"({_type}: {_key})";
}
