using System.Collections.Generic;
using UnityEngine;

public class Player : IValueTypeEnum<PlayerType>
{
    public PlayerType Type => _type;
    public Color Color => _visual.color;
    public Material Material => _visual.material;
    public Currencies Resources => _resources;

    private readonly PlayerType _type;
    private readonly PlayerVisual _visual;

    private readonly Currencies _resources;
    private Roads _roads;
    private readonly HashSet<Crossroad> _cities = new();

    Players _players; // test

    public Player(PlayerType type, PlayerVisual visual, Currencies resources, Players players)
    {
        _type = type;
        _visual = visual;
        _resources = resources;
        _players = players;// test
    }

    public void SetRoads(Roads roads) => _roads = roads.Initialize(_type, _visual.color);
    public void BuildRoad(CrossroadLink link)
    {
        _roads.BuildAndUnion(link);
        _resources.Pay(_roads.Cost);
    }
    public bool CanRoadBuilt(Crossroad crossroad) => _resources >= _roads.Cost && crossroad.CanRoadBuilt(_type);

    public void CityBuild(Crossroad crossroad, CityType type)
    {
        if(!_cities.Contains(crossroad) && crossroad.Build(_type, type, out Currencies cost))
        {
            _cities.Add(crossroad);
            _resources.Pay(cost);
        }
    }
    public void CityUpgrade(Crossroad crossroad)
    {
        if (_cities.Contains(crossroad) && crossroad.Upgrade(out Currencies cost))
            _resources.Pay(cost);
    }

    public override string ToString() => $"Player: {_type}";
}
