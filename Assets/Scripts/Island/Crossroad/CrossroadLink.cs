using UnityEngine;

public class CrossroadLink : IValueTypeEnum<LinkType>
{
    public KeyDouble Key => _key;
    public LinkType Type => _type;
    public bool IsWater => _isWater;
    public Vector3 Position => _middle;
    public PlayerType Owner {get => _owner; set => _owner = value; }

    public Crossroad Start => _start;
    public Crossroad End => _end;
   
    private Crossroad _start, _end;
    private PlayerType _owner;
    private readonly LinkType _type;
    private readonly bool _isWater;
    private readonly KeyDouble _key;
    private readonly Vector3 _middle;

    private static readonly Key[] NEAR_CROSS = { new(2, -1), new(2, 1), new(0, 2), new(-2, 1), new(-2, -1), new(0, -2) };

    public CrossroadLink(Crossroad crossA, Crossroad crossB, bool isWater)
    {
        _start = crossA; _end = crossB;
        _type = ToLinkType(_end - _start);

        if (!(_start.AddLink(this) && _end.AddLink(this)))
            return;

        _key = _start & _end;
        _isWater = isWater;
        _owner = PlayerType.None;
        _middle = (_start.Position + _end.Position) * 0.5f;

        // Local: ToLinkType(..)
        //=================================
        LinkType ToLinkType(Key key) => (LinkType)(System.Array.IndexOf(NEAR_CROSS, key) % 3);
    }

    public void SetStart(Crossroad cross)
    {
        if(_start != cross)
            (_start, _end) = (_end, _start);
    }

    public void RoadBuilt(PlayerType owner)
    {
        _owner = owner;
        _start.RoadBuilt(_type, owner);
        _end.RoadBuilt(_type, owner);
    }

    public Crossroad Other(Crossroad crossroad) => crossroad == _start ? _end : _start;
    
    public bool Contains(CrossroadData data) => data.Key == _start.Key || data.Key == _end.Key;

    public override string ToString() => $"({_type}: {_key})";
}
