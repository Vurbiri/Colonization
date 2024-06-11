using UnityEngine;

public class CrossroadLink 
{
    public KeyDouble Key => _key;
    public Vector3 Position => _middle;
    public PlayerType Owner {get => _owner; set => _owner = value; }

    public Crossroad Start => _start;
    public Crossroad End => _end;

    private Crossroad _start, _end;
    private PlayerType _owner;
    private readonly KeyDouble _key;
    private readonly Vector3 _middle;

    public CrossroadLink(KeyDouble key, Crossroad crossA, Crossroad crossB, PlayerType owner = PlayerType.None)
    {
        _key = key;

        _start = crossA; _end = crossB;
        _start.Links.Add(this);
        _end.Links.Add(this);
        
        _owner = owner;

        _middle = (_start.Position + _end.Position) * 0.5f;
    }

    public void SetStart(Crossroad cross)
    {
        if(_start != cross)
            (_start, _end) = (_end, _start);
    }

    public override string ToString() => $"({_key})";
}
