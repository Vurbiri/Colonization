using UnityEngine;

public class CrossroadLink 
{
    public KeyDouble Key => _key;
    public LinkType Type => _type;
    public Vector3 Position => _middle;
    public PlayerType Owner {get => _owner; set => _owner = value; }

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
        _type = _end - _start;

        if (!(_start.AddLink(_type, this) && _end.AddLink(_type, this)))
            return;

        _key = _start & _end;
        _owner = owner;
        _middle = (_start.Position + _end.Position) * 0.5f;
    }

    public CrossroadType GetCrossroadType(Crossroad cross) => _start == cross ? _end - _start : _start - _end;

    public void SetStart(Crossroad cross)
    {
        if(_start != cross)
            (_start, _end) = (_end, _start);
    }

    public void RoadBuilt(PlayerType owner)
    {
        _owner = owner;
        _start.RoadBuilt();
        _end.RoadBuilt();
    }

    public override string ToString() => $"({_type}: {_key})";
}
