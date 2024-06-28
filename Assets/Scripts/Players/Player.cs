using System.Collections.Generic;
using UnityEngine;

public class Player : IValueTypeEnum<PlayerType>
{
    public PlayerType Type => _type;
    public Color Color => _visual.color;
    public Material Material => _visual.material;

    private readonly PlayerType _type;
    private readonly PlayerVisual _visual;

    private Roads _roads;
    private readonly HashSet<Crossroad> _cities = new();

    Players _players; // test

    public Player(PlayerType type, PlayerVisual visual, Players players)
    {
        _type = type;
        _visual = visual;
        _players = players;// test
    }

    public void SetRoads(Roads roads) => _roads = roads.Initialize(_type, _visual.color);
    public void BuildRoad(CrossroadLink link) => _roads.BuildRoad(link);
    public bool CanRoadBuilt(Crossroad crossroad)
    {
        
        return crossroad.CanRoadBuilt(_type);
    }

    public bool CanCityUpgrade(Crossroad crossroad) => crossroad.CanCityUpgrade(_type);

    public void CityUpgrade(Crossroad crossroad)
    {
        if (_cities.Contains(crossroad))
        {
            if (crossroad.Upgrade())
            {
               
            }
            
        }
        else
        {
            if (crossroad.Build(_type))
            {
                _cities.Add(crossroad);
            }
        }

        _players.RandomPlayer();
    }

    public override string ToString() => $"Player: {_type}";
}
