using System.Collections.Generic;
using UnityEngine;

public class Road 
{
    public KeyDouble Key => _key;
    public Vector3 Position => _middle;
    public Player Owner => _owner;

    private Crossroad _crossA, _crossB;
    private Hexagon _hexA, _hexB;
    private Player _owner;
    private KeyDouble _key;
    private Vector3 _middle;
    
    private Road(Hexagon hexA, Hexagon hexB, Crossroad crossA, Crossroad crossB, Player owner)
    {
        _key = hexA & hexB;
        _hexA = hexA; _hexB = hexB;

        _crossA = crossA; _crossB = crossB;
        _crossA.Roads.Add(this);
        _crossB.Roads.Add(this);
        
        _owner = owner;

        _middle = (_crossA.Position + _crossB.Position) * 0.5f;
    }

    public void Build(Player owner)
    {
        _owner = owner;

        _hexA.BuildRoad(owner, _key.B_A);
        _hexB.BuildRoad(owner, _key.A_B);
    }

    public static Road Create(HashSet<Crossroad> crossroads, Hexagon hexA, Hexagon hexB, Player owner = Player.None)
    {
        if (crossroads.Count != 2 || (hexA.IsWater && hexB.IsWater))
            return null;

        Crossroad crossA, crossB;
        IEnumerator<Crossroad> enumerator = crossroads.GetEnumerator();
        enumerator.MoveNext();
        crossA = enumerator.Current;
        if (crossA.IsWater) 
            return null;
        enumerator.MoveNext();
        crossB = enumerator.Current;
        if (crossB.IsWater)
            return null;

        return new(hexA, hexB, crossA, crossB, owner);
    }

    public override string ToString() => $"({_hexA.Id}, {_hexB.Id})";
}
