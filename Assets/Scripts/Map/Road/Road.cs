using System;
using UnityEngine;

public class Road 
{
    public KeyDouble Key => _key;
    public Vector3 Position => _middle;
    public PlayerType Owner => _owner;

    public Crossroad CrossA => _crossA;
    public Crossroad CrossB => _crossB;

    private Crossroad _crossA, _crossB;
    private Hexagon _hexA, _hexB;
    private PlayerType _owner;
    private KeyDouble _key;
    private Vector3 _middle;
    private Func<Vector3, Vector3, bool> _actionBuildRoad;

    public Road(Hexagon hexA, Hexagon hexB, Crossroad crossA, Crossroad crossB, Func<Vector3, Vector3, bool> actionBuildRoad, PlayerType owner = PlayerType.None)
    {
        _key = hexA & hexB;
        _hexA = hexA; _hexB = hexB;

        _crossA = crossA; _crossB = crossB;
        _crossA.Roads.Add(this);
        _crossB.Roads.Add(this);
        
        _owner = owner;

        _actionBuildRoad = actionBuildRoad;

        _middle = (_crossA.Position + _crossB.Position) * 0.5f;
    }

    public void Build(PlayerType owner)
    {
        if (_owner != PlayerType.None) return;
        
        if(_actionBuildRoad(_crossA.Position, _crossB.Position))
            _owner = owner;
    }

    public override string ToString() => $"({_hexA.Id}, {_hexB.Id})";
}
